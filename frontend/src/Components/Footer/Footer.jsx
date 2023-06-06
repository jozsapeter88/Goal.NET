import React from 'react';
import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import './Footer.css';

function Footer() {
  return (
    <Navbar bg="dark" variant="dark" expand="lg" className="footer">
      <Container>
        <Navbar.Text >{new Date().getFullYear()} Goal.NET</Navbar.Text>
        <Nav className="ms-auto">
          <Nav.Link href="/about">Link</Nav.Link>
          <Nav.Link href="/contact">Link</Nav.Link>
        </Nav>
      </Container>
    </Navbar>
  );
}

export default Footer;
