import React from 'react';
import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import NavDropdown from 'react-bootstrap/NavDropdown';
import { useCookies } from 'react-cookie';
import { useNavigate } from "react-router-dom";
import './Menu.css';

function Menu() {
  const navigate = useNavigate();
  const [cookies, setCookie, removeCookie] = useCookies();
  const userName = cookies["username"];

  function onLogout() {
    removeCookie("username");
    removeCookie("token");
    navigate("/");
  }

  return (
    <Navbar bg="dark" variant="dark" expand="lg">
      <Container>
        <Navbar.Brand href="/Home">
          <img
            alt=""
            src="https://i.imgur.com/vH7MmvF.png"
            width="144"
            height="40"
            className="d-inline-block align-top logo-image"
          />
        </Navbar.Brand>
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav" className="justify-content-end">
          <Nav className="me-auto">
            <Nav.Link href="/teamManager">Team Manager</Nav.Link>
            <NavDropdown title= {`Welcome ${userName}`} id="basic-nav-dropdown">
              <NavDropdown.Item href="/Profile">Profile</NavDropdown.Item>
              <NavDropdown.Divider />
              <NavDropdown.Item href="/" onClick={onLogout}>Logout</NavDropdown.Item>
            </NavDropdown>
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
}

export default Menu;
