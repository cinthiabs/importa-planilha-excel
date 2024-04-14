const file = require('../fixtures/example.json')
const{id, nome,valor,quantidade, marca} = file;


describe('Import-Export', () => {
  beforeEach(()=> {
    cy.visit('/')
  })

  it('Validation of elements on the main page', () => {
    cy.get('.navbar-brand').should('contain','Importa Planilha Excel')
    cy.get('.form-control').should('be.visible')
    cy.get('.btn-primary').should('contain','Importar Excel')
    cy.get('#dropdownMenuButton').should('contain','Mais Opções').click()
    cy.get('a.dropdown-item').should('contain','Exportar dados')
    cy.get('button.dropdown-item').should('contain','Excluir todos os produtos')
    cy.get('table').should('be.visible')
  })
  it('Import wrong file', () => {
    const filePath = './cypress/fixtures/Produtos.pdf'
    cy.get('.form-control').selectFile(filePath)
    cy.get('.btn-primary').click()
    cy.get('tr > .text-center').should('contain','Sem produtos cadastrados!') 
    
  })

  it('Import and validate file', () => {
      const filePath = './cypress/fixtures/Produtos.xlsx'
      cy.get('.form-control').selectFile(filePath)
      cy.get('.btn-primary').click()

      cy.get('table').should('be.visible')

      cy.ValidateProduct(id)
      cy.ValidateProduct(nome)
      cy.ValidateProduct(valor)
      cy.ValidateProduct(quantidade)
      cy.ValidateProduct(marca)
  })
  it('Export File', () => {
    const filePath = 'cypress/downloads/produtos.xlsx'
    cy.get('#dropdownMenuButton').should('contain','Mais Opções').click()
    cy.get('a.dropdown-item').should('contain','Exportar dados').click()
    cy.readFile(filePath).should('exist'); 

})
  it('Delete a product', () => {
      cy.ClickDeleteButton(id)
      cy.ProductNoExist(id)
  })

  it('Delete all products', () => {
    cy.get('#dropdownMenuButton').should('contain','Mais Opções').click()
    cy.get('button.dropdown-item').should('contain','Excluir todos os produtos').click()
    cy.reload();
    cy.get('tr > .text-center').should('contain','Sem produtos cadastrados!') 
  })


})