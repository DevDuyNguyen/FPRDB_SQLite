SELECT * FROM sales.customers
WHERE state = 'CA' AND city='Glendora'
order by customer_id;

SELECT customer_id, first_name, last_name FROM sales.customers
WHERE state='NY' AND city='Rockville Centre'
UNION
SELECT customer_id, first_name, last_name FROM sales.customers
WHERE state = 'CA' AND city='Glendora';

