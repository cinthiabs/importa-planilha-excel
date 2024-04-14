Cypress.Commands.add('ValidateProduct', (text) => {
    cy.get('table').contains(text).closest('table').should('be.visible');
});


Cypress.Commands.add('ClickDeleteButton', (text) => {
    cy.get('table').contains(text).closest('tr').find(':nth-child(7) > #actions').click();
});

Cypress.Commands.add('ProductNoExist', (text) => {
    cy.get('table').should('not.contain',text)
});  