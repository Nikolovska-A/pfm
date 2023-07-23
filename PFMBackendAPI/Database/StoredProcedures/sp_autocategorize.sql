create procedure sp_autocategorize(IN catcode text, IN predicate text)
    language plpgsql
as
$$
BEGIN
    BEGIN

    EXECUTE 'UPDATE transactions SET "CatCode" = $1 WHERE "CatCode" IS NULL AND ' || predicate
    USING catcode;

    EXCEPTION
        WHEN others THEN
            RAISE NOTICE 'An error occurred: %', SQLERRM;
    END;
END;
$$;

alter procedure sp_autocategorize(text, text) owner to postgres;