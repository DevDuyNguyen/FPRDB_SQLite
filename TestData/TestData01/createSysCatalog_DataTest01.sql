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
CREATE TABLE fprdb_inDatabaseSQLFile (
    oid INTEGER PRIMARY KEY AUTOINCREMENT,
    fileName TEXT,
    fileContent TEXT
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

INSERT INTO fprdb_FuzzySet (oid, fuzzset_name, fuzzset_type_id) VALUES
(1, 'distFS1', 1),
(2, 'distFS2', 4),
(3, 'conFS1', 2);

INSERT INTO fprdb_DiscreteFuzzySet (oid, fuzzset_x, fuzzset_membership_degree) VALUES
(1, '1,2,3', '0,1,0'),
(2, 'a,bc,def', '1,1,1');

INSERT INTO fprdb_ContinousFuzzySet (
    oid, 
    fuzzset_bottom_left, 
    fuzzset_top_left, 
    fuzzset_top_right, 
    fuzzset_bottom_right
) VALUES (3, 1, 2, 3, 4);

INSERT INTO fprdb_RelationSchema (oid, relschema_name) VALUES
(1, 'sche1'),
(2, 'noRelSche'),
(3, 'referencingSche'),
(4, 'referencedSche');

INSERT INTO fprdb_Attribute (
    oid, 
    att_name, 
    att_type_id, 
    att_type_mod, 
    att_not_null, 
    att_relschema_id
) VALUES 
(1, 'id', 1, 0, 0, 1),
(2, 'intDistFS', 6, 0, 0, 1),
(3, 'id', 1, 0, 0, 2),
(4, 'id', 1, 0, 0, 4),
(5, 'age', 9, 0, 0, 4),
(6, 'id', 1, 0, 0, 3),
(7, 'fk_id', 1, 0, 0, 3),
(8, 'age', 9, 0, 0, 3);

INSERT INTO fprdb_Relation (
    oid, 
    rel_name, 
    rel_relation_schema
) VALUES 
(1, 'rel1', 1),
(2, 'referencingRel', 3),
(3, 'referencedRel', 4);

INSERT INTO fprdb_Constraint (
    oid, 
    con_name, 
    con_type, 
    con_relation_id, 
    con_referenced_relation_id, 
    con_attributes, 
    con_referenced_attributes, 
    con_relschema_id
) VALUES 
(1, 'pk_sche1', 'IDENTITY', NULL, NULL, 'id', NULL, 1),
(2, 'fk_refrencingRel_referencedRel', 'REFERENTIAL', 2, 3, 'fk_id', 'id', NULL),
(3, 'pk_referencingRel', 'IDENTITY', NULL, NULL, 'id', NULL, 3),
(4, 'pk_referencedRel', 'IDENTITY', NULL, NULL, 'id', NULL, 4),
(5, 'pk_noRelSche', 'IDENTITY', NULL, NULL, 'id', NULL, 2);

INSERT INTO FPRDB_Rel_FuzzSet (
    rel_oid, 
    fuzzset_oid, 
    no
) VALUES 
(1, 1, 1),
(2, 3, 1),
(3, 3, 3);

CREATE TABLE rel1 (
    id TEXT,
    intDistFS TEXT
);

CREATE TABLE referencedRel (
    id TEXT,
    age TEXT
);

CREATE TABLE referencingRel (
    id TEXT,
    fk_id TEXT,
    age TEXT
);

INSERT INTO rel1 (
    id, 
    intDistFS
) VALUES 
('{(1,[1,1])}', '{(distFS1,[1,1])}'),
('{(2,[1,1])}', '{(2,[1,1])}');

INSERT INTO referencedRel (
    id, 
    age
) VALUES 
('{(1,[1,1])}', '{(conFS1,[1,1])}'),
('{(2,[1,1])}', '{(conFS1,[0.5,0.5]), (conFS1,[0.5,0.5])}');

INSERT INTO referencingRel (id, fk_id, age) 
VALUES ('{(1,[1,1])}', '{(1,[1,1])}', '{(conFS1,[1,1])}');