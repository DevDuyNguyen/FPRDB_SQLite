SELECT * FROM sales.customers
WHERE state = 'CA' AND city='Glendora';

SELECT customer_id, first_name, last_name FROM sales.customers
WHERE state='NY' AND city='Rockville Centre'
UNION
SELECT customer_id, first_name, last_name FROM sales.customers
WHERE state = 'CA' AND city='Glendora';

SELECT product_id, product_name, category_name, list_price
FROM production.products p
INNER JOIN production.categories c 
	ON c.category_id = p.category_id
WHERE c.category_name='Mountain Bikes' AND p.list_price<=500;

