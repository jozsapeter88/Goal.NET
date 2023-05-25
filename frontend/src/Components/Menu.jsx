import { Nav, Navbar, Container } from "react-bootstrap";

const Menu = () => {
    return (
    <>
      <Navbar bg="dark" variant="dark">
        <Container>
          <Navbar.Brand href="/home">
            <img
              alt=""
              src="https://i.imgur.com/lavhqr8.png"
              width="30"
              height="30"
              className="d-inline-block align-top"
            />{' '}
            Goal.NET
          </Navbar.Brand>
          <Nav.Link href="/team-manager">
            Team manager
          </Nav.Link>
        </Container>
      </Navbar>
    </>
    )
}

export default Menu;