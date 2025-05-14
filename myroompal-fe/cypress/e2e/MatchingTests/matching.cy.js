
describe('Matching system', () => {
    beforeEach(() => {
        cy.visit('http://localhost:3000')

    })

    it('No results because nobody looks for a room a given location but saves it to database', () => {
        cy.get('[cy-data="logIn-btn"]').click();
        cy.origin("dev-jvk7nnjfyqlkrjn4.us.auth0.com", () => {
            cy.get('input[name="email"]').type("lomygara@cyclelove.cc");
            cy.get('input[name="password"]').type("!bw,EkpE5.5MB=9");
            cy.get('button[type="submit"]').click();
        })
        cy.get('[cy-data="Match"]', {timeout: 10000}).click();
        cy.get('[cy-data="search-location"]').type('Locatie bestaat niet');
        cy.get('[cy-data="A backyard"]').click()
        cy.get('[cy-data="find-matches"]').click()
        cy.get('[cy-data="0-results-found"]').should("exist")
    })

    it('Finds the previous user because it looks for a room at the same location', () => {
        cy.visit('http://localhost:3000')
        cy.get('[cy-data="logIn-btn"]').click();
        cy.origin("dev-jvk7nnjfyqlkrjn4.us.auth0.com", () => {
            cy.get('input[name="email"]').type("navafe4433@fundapk.com");
            cy.get('input[name="password"]').type("*DEk#i4VyJ]3**S");
            cy.get('button[type="submit"]').click();
        })
        cy.get('[cy-data="Match"]', {timeout: 10000}).click();
        cy.get('[cy-data="search-location"]').type('Locatie bestaat niet');
        cy.get('[cy-data="A backyard"]').click()
        cy.get('[cy-data="find-matches"]').click()

        cy.get(".user-card > :nth-child(1)", {timeout: 10000}).should("exist")
    })
})