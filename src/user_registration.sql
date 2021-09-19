CREATE TABLE public."Users"
(
    "FirstName" character varying(30) COLLATE pg_catalog."default" NOT NULL,
    "Id" uuid NOT NULL,
    "Email" character varying(30) COLLATE pg_catalog."default" NOT NULL,
    "LastName" character varying(30) COLLATE pg_catalog."default",
    CONSTRAINT "Users_pkey" PRIMARY KEY ("Id")
)

TABLESPACE pg_default;

ALTER TABLE public."Users"
    OWNER to postgres;






CREATE OR REPLACE FUNCTION public.getall(
	)
    RETURNS SETOF "Users" 
    LANGUAGE 'sql'

    COST 100
    VOLATILE 
    ROWS 1000
    
AS $BODY$  
    SELECT * FROM "Users";  
$BODY$;

ALTER FUNCTION public.getall()
    OWNER TO postgres;