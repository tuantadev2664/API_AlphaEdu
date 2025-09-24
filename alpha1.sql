CREATE TABLE schools (
    id UUID PRIMARY KEY,
    name TEXT NOT NULL,
    district TEXT,
    city TEXT,
    created_at TIMESTAMPTZ DEFAULT now()
);

CREATE TABLE academic_years (
    id UUID PRIMARY KEY,
    school_id UUID NOT NULL REFERENCES schools(id),
    name TEXT NOT NULL,
    start_date DATE,
    end_date DATE,
    CONSTRAINT uq_academic_year UNIQUE (school_id, name)
);

CREATE TABLE terms (
    id UUID PRIMARY KEY,
    academic_year_id UUID NOT NULL REFERENCES academic_years(id),
    code TEXT NOT NULL, -- ví dụ: S1, S2
    start_date DATE,
    end_date DATE,
    CONSTRAINT uq_term UNIQUE (academic_year_id, code)
);

CREATE TABLE grades (
    id UUID PRIMARY KEY,
    school_id UUID NOT NULL REFERENCES schools(id),
    level TEXT NOT NULL, -- ví dụ: primary, lower_secondary
    grade_number INT NOT NULL,
    CONSTRAINT uq_grade UNIQUE (school_id, grade_number)
);

CREATE TABLE users (
    id UUID PRIMARY KEY,
    role TEXT NOT NULL, -- ví dụ: student, teacher
    full_name TEXT NOT NULL,
    email TEXT UNIQUE,
    phone TEXT,
    school_id UUID NOT NULL REFERENCES schools(id),
    created_at TIMESTAMPTZ DEFAULT now()
);

CREATE TABLE classes (
    id UUID PRIMARY KEY,
    grade_id UUID NOT NULL REFERENCES grades(id),
    name TEXT NOT NULL,
    homeroom_teacher_id UUID REFERENCES users(id),
    CONSTRAINT uq_class UNIQUE (grade_id, name)
);

CREATE TABLE subjects (
    id UUID PRIMARY KEY,
    code TEXT NOT NULL,
    name TEXT NOT NULL,
    level TEXT NOT NULL,
    is_active BOOLEAN DEFAULT TRUE,
    CONSTRAINT uq_subject UNIQUE (code, level)
);

CREATE TABLE parent_students (
    parent_id UUID NOT NULL REFERENCES users(id),
    student_id UUID NOT NULL REFERENCES users(id),
    relationship TEXT,
    PRIMARY KEY (parent_id, student_id)
);

CREATE TABLE class_enrollments (
    id UUID PRIMARY KEY,
    class_id UUID NOT NULL REFERENCES classes(id),
    student_id UUID NOT NULL REFERENCES users(id),
    academic_year_id UUID NOT NULL REFERENCES academic_years(id),
    CONSTRAINT uq_class_enrollment UNIQUE (class_id, student_id, academic_year_id)
);

CREATE TABLE teacher_assignments (
    id UUID PRIMARY KEY,
    teacher_id UUID NOT NULL REFERENCES users(id),
    class_id UUID NOT NULL REFERENCES classes(id),
    subject_id UUID NOT NULL REFERENCES subjects(id),
    academic_year_id UUID NOT NULL REFERENCES academic_years(id),
    CONSTRAINT uq_teacher_assignment UNIQUE (teacher_id, class_id, subject_id, academic_year_id)
);

CREATE TABLE grade_components (
    id UUID PRIMARY KEY,
    class_id UUID NOT NULL REFERENCES classes(id),
    subject_id UUID NOT NULL REFERENCES subjects(id),
    term_id UUID NOT NULL REFERENCES terms(id),
    name TEXT NOT NULL,
    kind TEXT NOT NULL, -- ví dụ: quiz, test, midterm
    weight NUMERIC NOT NULL,
    max_score NUMERIC NOT NULL,
    position INT,
    CONSTRAINT uq_grade_component UNIQUE (class_id, subject_id, term_id, name)
);

CREATE TABLE assessments (
    id UUID PRIMARY KEY,
    grade_component_id UUID NOT NULL REFERENCES grade_components(id),
    title TEXT NOT NULL,
    due_date DATE,
    description TEXT
);

CREATE TABLE scores (
    id UUID PRIMARY KEY,
    assessment_id UUID NOT NULL REFERENCES assessments(id),
    student_id UUID NOT NULL REFERENCES users(id),
    score NUMERIC,
    is_absent BOOLEAN DEFAULT FALSE,
    comment TEXT,
    created_by UUID NOT NULL REFERENCES users(id),
    created_at TIMESTAMPTZ DEFAULT now(),
    updated_at TIMESTAMPTZ DEFAULT now(),
    CONSTRAINT uq_score UNIQUE (assessment_id, student_id)
);

CREATE TABLE behavior_notes (
    id UUID PRIMARY KEY,
    student_id UUID NOT NULL REFERENCES users(id),
    class_id UUID NOT NULL REFERENCES classes(id),
    term_id UUID NOT NULL REFERENCES terms(id),
    note TEXT,
    level TEXT,
    created_by UUID NOT NULL REFERENCES users(id),
    created_at TIMESTAMPTZ DEFAULT now()
);

CREATE TABLE messages (
    id UUID PRIMARY KEY,
    sender_id UUID NOT NULL REFERENCES users(id),
    receiver_id UUID NOT NULL REFERENCES users(id),
    content TEXT NOT NULL,
    created_at TIMESTAMPTZ DEFAULT now(),
    is_read BOOLEAN DEFAULT FALSE
);

CREATE TABLE announcements (
    id UUID PRIMARY KEY,
    sender_id UUID NOT NULL REFERENCES users(id), -- giáo viên hoặc admin
    class_id UUID REFERENCES classes(id),
    subject_id UUID REFERENCES subjects(id),
    title TEXT NOT NULL,
    content TEXT NOT NULL,
    created_at TIMESTAMPTZ DEFAULT now(),
    expires_at TIMESTAMPTZ,
    is_urgent BOOLEAN DEFAULT FALSE
);



-- Schools
INSERT INTO schools (id, name, district, city) VALUES
(gen_random_uuid(), 'Trường THCS Nguyễn Trãi', 'Ba Đình', 'Hà Nội'),
(gen_random_uuid(), 'Trường Tiểu học Kim Đồng', 'Hoàng Mai', 'Hà Nội');

-- Academic Years
INSERT INTO academic_years (id, school_id, name, start_date, end_date)
SELECT gen_random_uuid(), id, '2025-2026', '2025-09-01', '2026-05-31'
FROM schools;

-- Terms
INSERT INTO terms (id, academic_year_id, code, start_date, end_date)
SELECT gen_random_uuid(), id, 'S1', '2025-09-01', '2026-01-15' FROM academic_years;
INSERT INTO terms (id, academic_year_id, code, start_date, end_date)
SELECT gen_random_uuid(), id, 'S2', '2026-01-16', '2026-05-31' FROM academic_years;

-- Grades
INSERT INTO grades (id, school_id, level, grade_number)
SELECT gen_random_uuid(), id, 'lower_secondary', 6 FROM schools LIMIT 1;

-- Users (teacher, students, parents)
INSERT INTO users (id, role, full_name, email, phone, school_id)
SELECT gen_random_uuid(), 'teacher', 'Nguyễn Văn A', 'teacherA@school.vn', '0901111111', id FROM schools LIMIT 1;

INSERT INTO users (id, role, full_name, email, phone, school_id)
SELECT gen_random_uuid(), 'student', 'Trần Thị B', 'studentB@school.vn', '0902222222', id FROM schools LIMIT 1;

INSERT INTO users (id, role, full_name, email, phone, school_id)
SELECT gen_random_uuid(), 'student', 'Lê Văn C', 'studentC@school.vn', '0903333333', id FROM schools LIMIT 1;

INSERT INTO users (id, role, full_name, email, phone, school_id)
SELECT gen_random_uuid(), 'parent', 'Phụ huynh B', 'parentB@school.vn', '0904444444', id FROM schools LIMIT 1;

-- Classes
INSERT INTO classes (id, grade_id, name, homeroom_teacher_id)
SELECT gen_random_uuid(), g.id, '6A1', u.id
FROM grades g, users u
WHERE u.role = 'teacher'
LIMIT 1;

-- Subjects
INSERT INTO subjects (id, code, name, level)
VALUES
(gen_random_uuid(), 'MATH', 'Toán', 'lower_secondary'),
(gen_random_uuid(), 'LIT', 'Ngữ văn', 'lower_secondary');

-- Enroll students into class
INSERT INTO class_enrollments (id, class_id, student_id, academic_year_id)
SELECT gen_random_uuid(), c.id, u.id, ay.id
FROM classes c, users u, academic_years ay
WHERE u.role = 'student'
LIMIT 2;

-- Assign teacher to class/subject
INSERT INTO teacher_assignments (id, teacher_id, class_id, subject_id, academic_year_id)
SELECT gen_random_uuid(), t.id, c.id, s.id, ay.id
FROM users t, classes c, subjects s, academic_years ay
WHERE t.role = 'teacher'
LIMIT 1;

-- Grade components (ví dụ: Toán có kiểm tra 15p và 1 tiết)
INSERT INTO grade_components (id, class_id, subject_id, term_id, name, kind, weight, max_score, position)
SELECT gen_random_uuid(), c.id, s.id, t.id, 'KT 15 phút', 'quiz', 1, 10, 1
FROM classes c, subjects s, terms t
LIMIT 1;

INSERT INTO grade_components (id, class_id, subject_id, term_id, name, kind, weight, max_score, position)
SELECT gen_random_uuid(), c.id, s.id, t.id, 'KT 1 tiết', 'test', 2, 10, 2
FROM classes c, subjects s, terms t
LIMIT 1;

-- Assessments
INSERT INTO assessments (id, grade_component_id, title, due_date, description)
SELECT gen_random_uuid(), gc.id, 'Bài kiểm tra Toán số 1', '2025-10-15', 'Chương 1 Số học'
FROM grade_components gc
LIMIT 1;

-- Scores cho học sinh
INSERT INTO scores (id, assessment_id, student_id, score, created_by)
SELECT gen_random_uuid(), a.id, u.id, 8.5, t.id
FROM assessments a, users u, users t
WHERE u.role = 'student' AND t.role = 'teacher'
LIMIT 1;

INSERT INTO scores (id, assessment_id, student_id, score, created_by)
SELECT gen_random_uuid(), a.id, u.id, 6.5, t.id
FROM assessments a, users u, users t
WHERE u.role = 'student' AND t.role = 'teacher'
OFFSET 1 LIMIT 1;
