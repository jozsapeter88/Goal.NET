import React, { useEffect, useState } from 'react';
import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import './Footer.css';

function HealthCheck(){
  return fetch(process.env.REACT_APP_API_URL + "/healthcheck").then(res => res.status)
}

function Footer() {
  const [onlineMessage, setOnlineMessage] = useState(false);
  useEffect(() => {
    HealthCheck().then((statusCode) => {
      if (statusCode == 200) {
        setOnlineMessage(true);
      }
      else {
        setOnlineMessage(false);
      }
    })
  })
  let msgColor = onlineMessage ? "green" : "red";
  let statusMsg = onlineMessage ? "Server Online" : "Server offline";
  return (
    <Navbar bg="dark" variant="dark" expand="lg" className="footer">
      <Container>
        <Navbar.Text >{new Date().getFullYear()} Goal.NET</Navbar.Text>
        <Nav className="ms-auto">
          <Navbar.Text className={msgColor}>
            <div id='server-status'></div>
            {statusMsg}
            </Navbar.Text>
        </Nav>
      </Container>
    </Navbar>
  );
}

export default Footer;
