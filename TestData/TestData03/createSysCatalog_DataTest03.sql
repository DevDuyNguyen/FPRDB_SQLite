CREATE TABLE fprdb_RelationSchema (
    oid INTEGER PRIMARY KEY AUTOINCREMENT,
    relschema_name TEXT NOT NULL UNIQUE
);

CREATE TABLE fprdb_Relation (
    oid INTEGER PRIMARY KEY AUTOINCREMENT,
    rel_name TEXT NOT NULL UNIQUE,
    rel_relation_schema INTEGER NOT NULL,
    FOREIGN KEY (rel_relation_schema) REFERENCES fprdb_RelationSchema (oid)
);

CREATE TABLE fprdb_Type (
    oid INTEGER PRIMARY KEY AUTOINCREMENT,
    type_name TEXT NOT NULL UNIQUE,
    type_type TEXT NOT NULL
);

CREATE TABLE fprdb_Attribute (
    att_relschema_id INTEGER NOT NULL,
    oid INTEGER PRIMARY KEY AUTOINCREMENT,
    att_name TEXT NOT NULL,
    att_type_id INTEGER NOT NULL,
    att_type_mod INTEGER,
    att_not_null BOOLEAN,
    FOREIGN KEY (att_relschema_id) REFERENCES fprdb_RelationSchema (oid),
    FOREIGN KEY (att_type_id) REFERENCES fprdb_Type (oid)
);

CREATE TABLE fprdb_FuzzySet (
    oid INTEGER PRIMARY KEY AUTOINCREMENT,
    fuzzset_name TEXT NOT NULL UNIQUE,
    fuzzset_type_id INTEGER NOT NULL,
    FOREIGN KEY (fuzzset_type_id) REFERENCES fprdb_Type (oid)
);

CREATE TABLE fprdb_DiscreteFuzzySet (
    oid INTEGER PRIMARY KEY AUTOINCREMENT,
    fuzzset_x TEXT NOT NULL,
    fuzzset_membership_degree TEXT NOT NULL,
    FOREIGN KEY (oid) REFERENCES fprdb_FuzzySet (oid)
);

CREATE TABLE fprdb_ContinousFuzzySet (
    oid INTEGER PRIMARY KEY AUTOINCREMENT,
    fuzzset_bottom_left REAL NOT NULL,
    fuzzset_top_left REAL NOT NULL,
    fuzzset_top_right REAL NOT NULL,
    fuzzset_bottom_right REAL NOT NULL,
    FOREIGN KEY (oid) REFERENCES fprdb_FuzzySet (oid)
);

CREATE TABLE FPRDB_Rel_FuzzSet (
    rel_oid INTEGER,
    fuzzset_oid INTEGER,
    no INTEGER NOT NULL,
    PRIMARY KEY (rel_oid, fuzzset_oid),
    FOREIGN KEY (rel_oid) REFERENCES fprdb_Relation (oid),
    FOREIGN KEY (fuzzset_oid) REFERENCES fprdb_FuzzySet (oid)
);

CREATE TABLE fprdb_Constraint (
    oid INTEGER PRIMARY KEY AUTOINCREMENT,
    con_name TEXT NOT NULL UNIQUE,
    con_type TEXT NOT NULL,
    con_relation_id INTEGER,
    con_referenced_relation_id INTEGER,
    con_attributes TEXT NOT NULL,
    con_referenced_attributes TEXT,
    con_relschema_id INTEGER,
    FOREIGN KEY (con_relation_id) REFERENCES fprdb_Relation (oid),
    FOREIGN KEY (con_referenced_relation_id) REFERENCES fprdb_Relation (oid),
    FOREIGN KEY (con_relschema_id) REFERENCES fprdb_RelationSchema (oid)
);
INSERT INTO fprdb_Type (type_name, type_type) 
VALUES 
    ('INT', 'b'), 
    ('FLOAT', 'b'), 
    ('CHAR', 'b'), 
    ('VARCHAR', 'b'), 
    ('BOOLEAN', 'b'), 
    ('distFS_INT', 'fs'), 
    ('distFS_FLOAT', 'fs'), 
    ('distFS_TEXT', 'fs'), 
    ('contFS', 'fs');

CREATE TABLE patient1 (
    P_ID     TEXT,
    P_NAME   TEXT,
    P_AGE    TEXT,
    P_DISEASE TEXT,
    P_COST   TEXT
);

CREATE TABLE doctor1 (
    D_ID   TEXT,
    D_AGE  TEXT
);
CREATE TABLE doctor2 (
    D_NAME  TEXT,
    D_AGE   TEXT
);
CREATE TABLE doctor3 (
    D_NAME   TEXT,
    D_AGE    TEXT
);
CREATE TABLE diagnose1 (
    P_ID     TEXT,
    D_ID     TEXT,
    P_AGE    TEXT,
    DISEASE  TEXT
);
CREATE TABLE diagnose2 (
    P_ID     TEXT,
    D_ID     TEXT,
    P_AGE    TEXT,
    DISEASE  TEXT
);

INSERT INTO fprdb_FuzzySet (oid, fuzzset_name, fuzzset_type_id)
VALUES 
    (1,  'distFS1',           2),
    (2,  'distFS2',           2),
    (3,  'approx_15',         2),
    (4,  'about_2poi1',       2),
    (5,  'about_5poi1',       2),
    (6,  'left_close_to_6poin1', 2),
    (7,  'about_2',           1),
    (8,  'young',             2),
    (9,  'about_30',          1),
    (10, 'about_60',          1),
    (11, 'about_70',          1),
    (12, 'middle_aged',       2),
    (13, 'approx_30',         2);

INSERT INTO fprdb_DiscreteFuzzySet (oid, fuzzset_x, fuzzset_membership_degree) VALUES 
(1, '1.1,2,3', '1,0.1,0.5'),
(2, '1,2.1,3', '1,0.1,0.5'),
(4, '1.1,2.1,3.1', '0.5,1,0.5'),
(5, '4.1,5.1,6.1', '0.5,1,0.5'),
(6, '5.1,6.1', '0.8,1'),
(7, '1,2,3', '0.5,1,0.5'),
(9, '29,30,31', '0.5,1,0.5'),
(10, '58,59,60,61,62', '0.5,0.9,1,0.9,0.5'),
(11, '68,69,70,71,72', '0.5,0.9,1,0.9,0.5');

INSERT INTO fprdb_ContinousFuzzySet 
    (oid, fuzzset_bottom_left, fuzzset_top_left, fuzzset_top_right, fuzzset_bottom_right)
VALUES 
    (3,  10, 15, 15, 20),
    (8,   0,  0, 20, 35),
    (12, 20, 35, 45, 60),
    (13, 25, 30, 30, 35);
	
INSERT INTO fprdb_RelationSchema (oid, relschema_name)
VALUES 
    (1, 'PATIENT'),
    (2, 'DOCTOR1'),
    (3, 'DOCTOR2'),
    (4, 'DIAGNOSE'),
    (5, 'DOCTOR3');
	
INSERT INTO fprdb_Attribute 
    (oid, att_name, att_type_id, att_type_mod, att_not_null, att_relschema_id)
VALUES 
    (1,  'P_ID',       4, 50, 0, 1),
    (2,  'P_NAME',     4, 50, 0, 1),
    (3,  'P_AGE',      9,  0, 0, 1),
    (4,  'P_DISEASE',  4, 50, 0, 1),
    (5,  'P_COST',     6,  0, 0, 1),
    (6,  'D_ID',       4, 50, 0, 2),
    (7,  'D_AGE',      9,  0, 0, 2),
    (8,  'D_NAME',     4, 50, 0, 3),
    (9,  'D_AGE',      9,  0, 0, 3),
    (10, 'P_ID',       4, 50, 0, 4),
    (11, 'D_ID',       4, 50, 0, 4),
    (12, 'P_AGE',      9,  0, 0, 4),
    (13, 'DISEASE',    4, 50, 0, 4),
    (14, 'D_NAME',       4, 50, 0, 5),
    (15, 'D_AGE',      6,  0, 0, 5);
	
INSERT INTO fprdb_Constraint 
    (oid, con_name, con_type, con_relation_id, con_referenced_relation_id, 
     con_attributes, con_referenced_attributes, con_relschema_id)
VALUES 
    (1, 'pk_PATIENT',   'IDENTITY', NULL, NULL, 'P_ID',     NULL, 1),
    (2, 'pk_DOCTOR1',   'IDENTITY', NULL, NULL, 'D_ID',     NULL, 2),
    (3, 'pk_DOCTOR2',   'IDENTITY', NULL, NULL, 'D_NAME',   NULL, 3),
    (4, 'pk_DIAGNOSE',  'IDENTITY', NULL, NULL, 'P_ID,D_ID', NULL, 4),
    (5, 'pk_DOCTOR3',   'IDENTITY', NULL, NULL, 'D_ID',     NULL, 5);
	
INSERT INTO fprdb_Relation (oid, rel_name, rel_relation_schema)
VALUES 
    (1, 'patient1', 1),
    (2, 'doctor1',  2),
    (3, 'doctor2',  3),
    (4, 'diagnose1', 4),
    (5, 'diagnose2', 4),
    (6, 'doctor3',  5);
	
--==========================
INSERT INTO patient1 (P_ID, P_NAME, P_AGE, P_DISEASE, P_COST)
VALUES 
    ('{("PT111",[1,1])}', '{("N.V.Ha",[1,1])}', '{(65, [1,1])}', 
     '{("lung cancer", [0.5, 0.5]), ("tuberculosis", [0.5, 0.5])}', 
     '{(300, [0.5, 0.5]), (350, [0.5, 0.5])}'),

    ('{("PT112",[1,1])}', '{("T.V.Son",[1,1])}', '{(young, [1,1])}', 
     '{("hepatitis", [0.45, 0.65]), ("cirrhosis", [0.45, 0.65])}', 
     '{(about_60, [0.4, 0.6]), (about_70, [0.4, 0.6])}'),

    ('{("PT113",[1,1])}', '{("L.T.Lan",[1,1])}', '{(middle_aged, [1,1])}', 
     '{("cholecystitis", [1,1])}', 
     '{(8, [1,1])}');	
	
INSERT INTO doctor1 (D_ID, D_AGE)
VALUES 
    ('{("DT005",[1,1])}', '{(middle_aged,[1,1])}'),
    ('{("DT093",[1,1])}', '{(approx_30,[1,1])}'),
    ('{("DT102",[1,1])}', '{(55,[0.5,0.5]), (56,[0.5,0.5])}');
	
-- ==================== doctor2 ====================

INSERT INTO doctor2 (D_NAME, D_AGE)
VALUES 
    ('{("L.V.Cuong",[1,1])}', '{(30,[0.4,0.6]), (31,[0.4,0.6])}'),
    ('{("N.V.Hung",[1,1])}', '{(middle_aged,[1,1])}'),
    ('{("N.T.Dat",[1,1])}', '{(54,[0.5,0.5]), (55,[0.5,0.5])}');


-- ==================== diagnose1 ====================

INSERT INTO diagnose1 (P_ID, D_ID, P_AGE, DISEASE)
VALUES 
    ('{("PT226",[1,1])}', '{("DT093",[1,1])}', '{(65,[1,1])}', 
     '{("lung cancer",[0.4,0.6]), ("tuberculosis",[0.4,0.6])}'),

    ('{("PT234",[1,1])}', '{("DT102",[1,1])}', '{(approx_15,[1,1])}', 
     '{("hepatitis",[0.5,0.5]), ("cirrhosis",[0.5,0.5])}');


-- ==================== diagnose2 ====================

INSERT INTO diagnose2 (P_ID, D_ID, P_AGE, DISEASE)
VALUES 
    ('{("PT383",[1,1])}', '{("DT102",[1,1])}', '{(69,[0.5,0.5]), (70,[0.5,0.5])}', 
     '{("lung cancer",[1,1])}'),

    ('{("PT234",[1,1])}', '{("DT102",[1,1])}', '{(young,[1,1])}', 
     '{("hepatitis",[0.4,0.6]), ("gall-stone",[0.4,0.6])}'),

    ('{("PT242",[1,1])}', '{("DT025",[1,1])}', '{(middle_aged,[1,1])}', 
     '{("cholecystitis",[1,1])}');

	
	