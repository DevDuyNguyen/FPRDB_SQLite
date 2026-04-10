CREATE TABLE fprdb_RelationSchema (
    oid INTEGER, --PRIMARY KEY AUTOINCREMENT,
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