-- ==========================================================================
-- Table creation
-- ==========================================================================

CREATE TABLE measurement_unit (
	name text NOT NULL,
	PRIMARY KEY (name)
);

CREATE TABLE blast_project (
	id uuid NOT NULL,
	created_at timestamp NOT NULL,
	updated_at timestamp NOT NULL,
	Name text NOT NULL,
	description text,
	measurement_unit text REFERENCES measurement_unit(name) NOT NULL,
	utc_offset real REFERENCES time_zone(utc_offset) NOT NULL,
	local_transformation jsonb,
	utc_offset_hours integer NOT NULL,
	utc_offset_minutes integer NOT NULL,
	local_timezone_id_name text NOT NULL,
	local_is_daylight_saving_time boolean NOT NULL,
	PRIMARY KEY (id)
);

CREATE TABLE pattern_type (
	name text NOT NULL,
	PRIMARY KEY (name)
);

CREATE TABLE validation_state (
	name text NOT NULL,
	PRIMARY KEY (name)
);

CREATE TABLE fragmentation (
	id integer NOT NULL,
	top_size double precision, 
	p10 double precision, 
	p20 double precision, 
	p30 double precision, 
	p40 double precision, 
	p50 double precision, 
	p60 double precision, 
	p70 double precision, 
	p80 double precision, 
	p90 double precision, 
	PRIMARY KEY (id)
);

CREATE TABLE pattern (
	id uuid NOT NULL,
	created_at timestamp NOT NULL,
	updated_at timestamp NOT NULL,
	Name text NOT NULL,
	project_id uuid REFERENCES blast_project(id) NOT NULL,
	pattern_type text REFERENCES pattern_type(name) NOT NULL,
	pattern_purpose text NOT NULL,
	description text,
	total_hole_count integer NOT NULL,
	face_angle double precision NOT NULL,
	sub_drill double precision NOT NULL,
	bench text,
	pit text,
	phase text,
	area double precision,
	volume double precision,
	average_hole_length double precision,
	total_hole_length double precision,
	hole_usage text NOT NULL,
	pattern_template_name text,
	charging_template_name text,
	is_electric_blast boolean NOT NULL,
	max_holes_fired integer,
	max_weight_fired double precision,
	powder_factor double precision NOT NULL,
	rock_sg double precision NOT NULL,
	metric_scores jsonb,
	score double precision,
	validation_state text REFERENCES validation_state(name) NOT NULL,
	design_framentation_id integer REFERENCES fragmentation(id),
	actual_framentation_id integer REFERENCES fragmentation(id),
	design_boundary geometry NOT NULL,
	actual_boundary geometry,
	geology_code text,
	PRIMARY KEY (id)
);

CREATE TABLE hole (
	id uuid NOT NULL,
	created_at timestamp NOT NULL,
	updated_at timestamp NOT NULL,
	name text NOT NULL,
	hole_state text NOT NULL,
	area_of_influence double precision NOT NULL,
	volume_of_influence double precision NOT NULL, 
	hole_usage text NOT NULL,
	layout_type text NOT NULL,
	drill_pattern_id uuid REFERENCES pattern(id) NOT NULL,
	blast_pattern_id uuid REFERENCES pattern(id),
	design_azimuth double precision NOT NULL,
	design_dip double precision NOT NULL,
	design_length double precision NOT NULL,
	design_diameter double precision NOT NULL,
	design_bench_collar double precision,
	design_bench_toe double precision,
	design_sub_drill double precision NOT NULL,
	design_collar geometry NOT NULL,
	design_toe geometry NOT NULL,
	actual_azimuth double precision,
	actual_dip double precision,
	actual_length double precision,
	actual_collar geometry,
	actual_toe geometry,
	accuracy double precision,
	length_accuracy double precision,
	design_charge_thickness double precision,
	design_charge_weight double precision,
	actual_charge_thickness double precision,
	actual_charge_weight double precision,
	charge_template_name text,
	fragment_size double precision,
	powder_factor double precision,
	validation_state text REFERENCES validation_state(name) NOT NULL,
	actual_framentation_id integer REFERENCES fragmentation(id),
	raw_framentation jsonb,
	design_trace geometry,
	actual_trace geometry,
	geology_code text,
	PRIMARY KEY (id)
);

CREATE TABLE profile_type (
	name text NOT NULL,
	PRIMARY KEY (name)
);

CREATE TABLE charging_interval (
	id uuid NOT NULL,
	created_at timestamp,
	updated_at timestamp,
	profile_type text REFERENCES profile_type(name) NOT NULL,
	hole_id uuid REFERENCES hole(id) NOT NULL,
	"from" double precision NOT NULL, 
	"to" double precision NOT NULL, 
	consumable text NOT NULL,
	amount double precision NOT NULL, 
	deck text,
	PRIMARY KEY (Id)
);

-- may need to add major, minor, patch as separate columns later
CREATE TABLE database_version (
	full_version text NOT NULL,
	PRIMARY KEY (full_version)
);

-- ==========================================================================
-- Insert system defined enum values
-- ==========================================================================

INSERT INTO measurement_unit VALUES
    ('Imperial'),
	('Metric');

INSERT INTO pattern_type VALUES
    ('Drill'),
	('Blast'),
	('DrillBlast');
	
INSERT INTO validation_state VALUES
    ('Unvalidated'),
	('Valid'),
	('Invalid');
	
INSERT INTO profile_type VALUES
    ('Actual'),
	('Design');
	
INSERT INTO database_version VALUES
    ('0.5.0');
	