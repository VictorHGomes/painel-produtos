import api from './api'

export async function listarProdutos(filtros) {
  const resposta = await api.get('/api/produtos', { params: filtros })
  return resposta.data
}

export async function obterProduto(id) {
  const resposta = await api.get(`/api/produtos/${id}`)
  return resposta.data
}

export async function criarProduto(produto) {
  const resposta = await api.post('/api/produtos', produto)
  return resposta.data
}
