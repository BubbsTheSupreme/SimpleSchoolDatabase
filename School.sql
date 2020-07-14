-- CURRENT STUDENTS
CREATE TABLE "Students"
(
    "StudentId" INTEGER PRIMARY KEY,
    "FirstName" VARCHAR(15) NOT NULL,
    "LastName" VARCHAR(20) NOT NULL,
    "Email" VARCHAR(35) NOT NULL,
    "Gender" VARCHAR(6) NOT NULL,
    "Major" VARCHAR(20) NULL
);

-- NEW STUDENTS
CREATE TABLE "Applicants"
(
    "ApplicantId" INTEGER PRIMARY KEY,
    "FirstName" VARCHAR(15) NOT NULL,
    "LastName" VARCHAR(20) NOT NULL,
    "Email" VARCHAR(35) NOT NULL,
    "Gender" VARCHAR(6) NOT NULL,
    "Major" VARCHAR(20) NULL,
    "ApplicationStatus" VARCHAR(8) NULL
);

-- EXPELLED STUDENTS
CREATE TABLE "ExpelledStudents"
(
    "StudentId" INTEGER PRIMARY KEY,
    "FirstName" VARCHAR(15) NOT NULL,
    "LastName" VARCHAR(20) NOT NULL,
    "Email" VARCHAR(35) NOT NULL,
    "Gender" VARCHAR(6) NOT NULL,
    "Major" VARCHAR(20) NULL,
    "Reason" VARCHAR(500) NOT NULL
);

-- ADMIN LOGIN DATA
CREATE TABLE "Administrators"
(
    "AdminId" INTEGER PRIMARY KEY,
    "AdminName" VARCHAR(15) NOT NULL,
    "Password" VARCHAR(65) NOT NULL
);

-- CREATING DEFAULT ADMIN
INSERT INTO "Administrators" 
(
    "AdminId", 
    "AdminName", 
    "Password"
)
VALUES(
    0,
    "admin",
    "mOiGq+WQSiaQSoM0P3os6scKmxr0mdit6QE33OJ5T9g="
);