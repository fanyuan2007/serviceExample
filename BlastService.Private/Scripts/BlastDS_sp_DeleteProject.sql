CREATE OR REPLACE PROCEDURE "DeleteProject" (project_id uuid)
LANGUAGE plpgsql
AS $$

BEGIN
	-- get a list of pattern ids associated with the project to be deleted
	CREATE TEMP TABLE temp_pattern_ids
	(
		patternid uuid
	);

	INSERT INTO temp_pattern_ids
	SELECT "Id"
	FROM "Patterns"
	WHERE "ProjectId" = project_id;
	
	-- get a list of hole ids associated with the patterns to be deleted
	CREATE TEMP TABLE temp_hole_ids
	(
		holeid uuid
	);

	INSERT INTO temp_hole_ids
	SELECT "Id"
	FROM "Holes"
	INNER JOIN temp_pattern_ids
	ON patternid = "DrillPatternId";

	INSERT INTO temp_hole_ids
	SELECT "Id"
	FROM "Holes"
	INNER JOIN temp_pattern_ids
	ON patternid = "BlastPatternId";

	-- get a list of fragment ids associated with the patterns/holes to be deleted
	CREATE TEMP TABLE temp_fragment_ids
	(
		fragid uuid
	);

	INSERT INTO temp_fragment_ids
	SELECT f."Id"
	FROM "Fragmentation" AS f
	INNER JOIN "Holes" AS h
	ON f."Id" = h."ActualFragmentId"
	WHERE h."Id" in
		(SELECT holeid 
		 FROM temp_hole_ids);

	INSERT INTO temp_fragment_ids
	SELECT f."Id"
	FROM "Fragmentation" AS f
	INNER JOIN "Patterns" AS p
	ON f."Id" = p."ActualFragmentId"
	WHERE p."Id" in
		(SELECT patternid 
		 FROM temp_pattern_ids);

	INSERT INTO temp_fragment_ids
	SELECT f."Id"
	FROM "Fragmentation" AS f
	INNER JOIN "Patterns" AS p
	ON f."Id" = p."DesignFragmentId"
	WHERE p."Id" in
		(SELECT patternid 
		 FROM temp_pattern_ids);

	-- delete charging intervals assoicated with the holes to be deleted
	DELETE FROM "ChargingIntervals"
	WHERE "HoleId" in
		(SELECT holeid
		 FROM temp_hole_ids);

	-- delete holes associated with the project to be deleted
	DELETE FROM "Holes"
	WHERE "Id" in 
		(SELECT holeid 
		 FROM temp_hole_ids);
	
	-- delete patterns associated with the project to be deleted
	DELETE FROM "Patterns"
	WHERE "Id" in 
		(SELECT patternid 
		 FROM temp_pattern_ids);

	-- delete fragmentation data associated with the patterns/holes to be deleted
	DELETE FROM "Fragmentation"
	WHERE "Id" in 
		(SELECT fragid
		 FROM temp_fragment_ids);

	-- delete the project
	DELETE FROM "Projects"
	WHERE "Id" = project_id;

	-- drop the temp tables
	DROP TABLE temp_fragment_ids;
	DROP TABLE temp_hole_ids;
	DROP TABLE temp_pattern_ids;
		
	EXCEPTION 
		WHEN OTHERS THEN
	
			RAISE NOTICE 'There is an error during project deletion. '
					 'The transaction is in an uncommittable state. '
					 'Transaction was rolled back';
	
END;
$$

