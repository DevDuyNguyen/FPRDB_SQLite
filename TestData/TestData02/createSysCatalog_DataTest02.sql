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

CREATE TABLE rel1 (
	id TEXT,
	att1 TEXT,
	att2 TEXT,
	att3 TEXT
);

CREATE TABLE rel2 (
	id TEXT,
	name TEXT,
	age TEXT,
	gender TEXT
);

INSERT INTO fprdb_FuzzySet (oid, fuzzset_name, fuzzset_type_id)
VALUEs 
    (1, 'distFS1', 2),
    (2, 'distFS2', 2),
    (3, 'approx_15', 2),
    (4, 'about_2point1', 2),
    (5, 'about_5point1', 2),
    (6, 'left_close_to_6point1', 2),
    (7, 'about_2', 1),
    (8, 'young', 2),
    (9, 'about_30', 1);

INSERT INTO fprdb_DiscreteFuzzySet (oid, fuzzset_x, fuzzset_membership_degree)
VALUES 
    (1, '1.1,2,3', '1.0,1.0,0.5'),
    (2, '1,2.1,3', '1.0,1.0,0.5'),
    (4, '1.1,2.1,3.1', '0.5,1.0,0.5'),
    (5, '4.1,5.1,6.1', '0.5,1.0,0.5'),
    (6, '5.1,6.1', '0.8,1'),
    (7, '1,2,3', '0.5,1.0,0.5'),
    (9, '29,30,31', '0.5,1.0,0.5');

INSERT INTO fprdb_ContinousFuzzySet 
    (oid, fuzzset_bottom_left, fuzzset_top_left, fuzzset_top_right, fuzzset_bottom_right)
VALUES 
    (3, 10, 15, 15, 20),
    (8, 0, 0, 20, 35);
	
INSERT INTO fprdb_RelationSchema (oid, relschema_name)
VALUES 
    (1, 'sche1'),
    (2, 'sche2');
	
INSERT INTO fprdb_Attribute 
    (oid, att_name, att_type_id, att_type_mod, att_not_null, att_relschema_id)
VALUES 
    (1, 'id',    1, 0, 0, 1),
    (2, 'att1',  6, 0, 0, 1),
    (3, 'att2',  7, 0, 0, 1),
    (4, 'att3',  9, 0, 0, 1),
    (5, 'id',    1, 0, 0, 2),
    (6, 'name',  4, 50, 0, 2),
    (7, 'age',   9, 0, 0, 2),
    (8, 'gender',5, 0, 0, 2);
	
INSERT INTO fprdb_Constraint 
    (oid, con_name, con_type, con_relation_id, con_referenced_relation_id, 
     con_attributes, con_referenced_attributes, con_relschema_id)
VALUES 
    (1, 'pk_sche1', 'IDENTITY', NULL, NULL, '"id"', NULL, 1),
    (2, 'pk_sche2', 'IDENTITY', NULL, NULL, '"id"', NULL, 2);
	
INSERT INTO fprdb_Relation 
    (oid, rel_name, rel_relation_schema)
VALUES 
    (1, 'rel1', 1),
    (2, 'rel2', 2);
	
INSERT INTO rel1(id, att1, att2, att3)
VALUES
('{(1,[1,1])}', '{(about_2,[0.4,0.6]), (about_30,[0.4,0.6])}', '{(about_5point1,[0.4,0.6]), (1,[0.4,0.6])}', '{(young,[0.4,0.6]), (approx_15,[0.4,0.6])}'),
('{(2,[1,1])}', '{(1,[0.4,0.5]), (about_30,[0.4,0.6])}', '{(31,[0.4,0.6]), (30,[0.4,0.6])}', '{(young,[0.4,0.6]), (22,[0.4,0.6])}');

INSERT INTO rel2(id,name,age,gender)
VALUES
('{(1,[1,1])}', '{("d1",[1,1])}', '{(1,[1,1])}', '{(True,[1,1])}');

	
	