-- =====================================================================
-- 04_consultas.sql
-- Consultas de estudo e conferência (somente leitura, não altera dados).
-- =====================================================================

-- 1) Listagem de produtos com o nome da categoria (JOIN)
SELECT p."Id",
       p."Nome",
       c."Nome" AS "Categoria",
       p."Preco",
       p."Estoque",
       p."Ativo"
FROM "Produtos" p
JOIN "Categorias" c ON c."Id" = p."CategoriaId"
ORDER BY c."Nome", p."Nome";

-- 2) Resumo por categoria: quantidade de produtos e valor total em estoque
--    (JOIN + GROUP BY). Considera apenas produtos ativos.
SELECT c."Nome"                                      AS "Categoria",
       COUNT(p."Id")                                 AS "QtdProdutos",
       COALESCE(SUM(p."Estoque"), 0)                 AS "UnidadesEmEstoque",
       COALESCE(SUM(p."Preco" * p."Estoque"), 0)     AS "ValorTotalEstoque"
FROM "Categorias" c
LEFT JOIN "Produtos" p
       ON p."CategoriaId" = c."Id"
      AND p."Ativo" = TRUE
GROUP BY c."Id", c."Nome"
ORDER BY "ValorTotalEstoque" DESC;

-- 3) Somente categorias com mais de 50 mil reais em estoque (HAVING)
SELECT c."Nome"                              AS "Categoria",
       SUM(p."Preco" * p."Estoque")          AS "ValorTotalEstoque"
FROM "Categorias" c
JOIN "Produtos" p ON p."CategoriaId" = c."Id"
WHERE p."Ativo" = TRUE
GROUP BY c."Id", c."Nome"
HAVING SUM(p."Preco" * p."Estoque") > 50000
ORDER BY "ValorTotalEstoque" DESC;

-- 4) Produtos que precisam de atenção: sem estoque ou inativos
SELECT p."Id",
       p."Nome",
       c."Nome" AS "Categoria",
       p."Estoque",
       p."Ativo"
FROM "Produtos" p
JOIN "Categorias" c ON c."Id" = p."CategoriaId"
WHERE p."Estoque" = 0
   OR p."Ativo" = FALSE
ORDER BY p."Nome";
