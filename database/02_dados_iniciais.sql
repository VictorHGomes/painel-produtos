-- =====================================================================
-- 02_dados_iniciais.sql
-- Carga inicial: 5 categorias e 30 produtos.
-- Executar UMA vez, depois que as tabelas existirem (migration aplicada).
-- Rodar de novo duplica os dados.
-- =====================================================================

BEGIN;

-- 1) Categorias
-- O Id é gerado automaticamente (identity), então não informamos.
INSERT INTO "Categorias" ("Nome") VALUES
    ('Eletrônicos'),
    ('Informática'),
    ('Eletrodomésticos'),
    ('Papelaria'),
    ('Móveis');

-- 2) Produtos
-- Em vez de digitar o Id da categoria (que pode variar), informamos o NOME
-- da categoria e deixamos o JOIN descobrir o Id correspondente.
INSERT INTO "Produtos"
    ("Nome", "Descricao", "Preco", "Estoque", "CategoriaId", "Ativo", "DataCriacao")
SELECT v.nome, v.descricao, v.preco, v.estoque, c."Id", v.ativo, NOW()
FROM (VALUES
    -- Eletrônicos
    ('Smartphone 128GB',                 'Tela de 6,5 polegadas e câmera dupla',        1899.90, 25,  'Eletrônicos',      TRUE),
    ('Fone de Ouvido Bluetooth',         'Sem fio, com cancelamento de ruído',           249.90, 80,  'Eletrônicos',      TRUE),
    ('Smart TV 50 Polegadas 4K',         'Sistema com aplicativos de streaming',        2799.00, 12,  'Eletrônicos',      TRUE),
    ('Caixa de Som Portátil',            'Resistente à água, bateria de 12 horas',       189.90, 40,  'Eletrônicos',      TRUE),
    ('Smartwatch Fitness',               'Monitor de batimentos e GPS',                  399.90, 30,  'Eletrônicos',      TRUE),
    ('Carregador Portátil 10000mAh',     NULL,                                           119.90,  0,  'Eletrônicos',      TRUE),

    -- Informática
    ('Notebook 15 Polegadas 16GB',       'Processador de última geração, SSD de 512GB', 4299.00, 10,  'Informática',      TRUE),
    ('Mouse sem Fio',                    'Conexão USB, 1600 DPI',                         59.90, 150, 'Informática',      TRUE),
    ('Teclado Mecânico',                 'Switch azul, layout ABNT2',                    289.90, 45,  'Informática',      TRUE),
    ('Monitor 24 Polegadas Full HD',     'Painel IPS, 75Hz',                             749.90, 20,  'Informática',      TRUE),
    ('Webcam Full HD',                   'Microfone embutido',                           179.90, 35,  'Informática',      TRUE),
    ('SSD 1TB NVMe',                     NULL,                                           429.90, 28,  'Informática',      TRUE),

    -- Eletrodomésticos
    ('Air Fryer 4 Litros',               'Fritadeira sem óleo com timer',                349.90, 33,  'Eletrodomésticos', TRUE),
    ('Liquidificador 900W',              'Copo de vidro com 6 velocidades',              159.90, 50,  'Eletrodomésticos', TRUE),
    ('Cafeteira Elétrica',               'Capacidade para 30 xícaras',                   129.90, 42,  'Eletrodomésticos', TRUE),
    ('Micro-ondas 20 Litros',            'Painel digital com 8 funções',                 499.00, 18,  'Eletrodomésticos', TRUE),
    ('Aspirador de Pó Vertical',         'Sem fio, com filtro lavável',                  379.90, 15,  'Eletrodomésticos', TRUE),
    ('Ventilador de Mesa',               NULL,                                           149.90, 60,  'Eletrodomésticos', TRUE),

    -- Papelaria
    ('Caderno Universitário 200 Folhas', 'Capa dura, 10 matérias',                        24.90, 300, 'Papelaria',        TRUE),
    ('Caneta Esferográfica Azul (50 un)', 'Ponta média de 1.0 mm',                        39.90, 120, 'Papelaria',        TRUE),
    ('Agenda 2026',                      'Modelo descontinuado',                          44.90,  0,  'Papelaria',        FALSE),
    ('Marcador de Texto (kit 6 cores)',  'Ponta chanfrada',                               19.90, 200, 'Papelaria',        TRUE),
    ('Pasta Arquivo A4',                 NULL,                                            14.90, 250, 'Papelaria',        TRUE),
    ('Calculadora Científica',           '240 funções',                                   89.90, 55,  'Papelaria',        TRUE),

    -- Móveis
    ('Cadeira de Escritório Ergonômica', 'Apoio lombar e altura ajustável',              899.90, 14,  'Móveis',           TRUE),
    ('Mesa de Escritório 120cm',         'Tampo em MDF com passa-fios',                  549.90,  9,  'Móveis',           TRUE),
    ('Estante 5 Prateleiras',            'Estrutura em MDP',                             329.90, 11,  'Móveis',           TRUE),
    ('Poltrona Reclinável',              'Linha retirada do catálogo',                  1299.00,  5,  'Móveis',           FALSE),
    ('Gaveteiro com 3 Gavetas',          'Com rodízios',                                 279.90, 16,  'Móveis',           TRUE),
    ('Suporte para Monitor',             NULL,                                            99.90, 70,  'Móveis',           TRUE)
) AS v(nome, descricao, preco, estoque, categoria, ativo)
JOIN "Categorias" c ON c."Nome" = v.categoria;

COMMIT;
