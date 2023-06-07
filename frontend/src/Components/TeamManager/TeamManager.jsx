import React, { useState, useEffect } from "react";
import { Container, Row, Col, Card } from "react-bootstrap";
import Menu from "../Menu/Menu";
import CreateSection from "./CreateSection";
import ManageSection from "./ManageSection";
import useCookies from "react-cookie/cjs/useCookies";

const TeamManager = () => {
  const [teams, setTeams] = useState([]);
  const [loading, setLoading] = useState(true);
  const [cookies, setCookies] = useCookies();

  const fetchTeamsOfUser = (signal) => {
    return fetch(`http://localhost:3000/api/teams/user/getTeams`, {
      headers: {
        Authorization: "Bearer " + cookies["token"],
      },
      signal,
    }).then((res) => res.json());
  };

  useEffect(() => {
    const controller = new AbortController();
    fetchTeamsOfUser(controller.signal)
      .then((teamsData) => {
        setTeams(teamsData);
        console.log(teamsData);
        setLoading(false);
      })
      .catch((error) => {
        if (error.name !== "AbortError") {
          setTeams([]);
          throw error;
        }
      });

    return () => controller.abort();
  }, []);

  if (Object.keys(teams).length === 0) {
    return (
      <>
        <Menu />
        <Card bg="secondary" style={{ color: "white" }}>
          <Card.Body>
            <Card.Title>No teams found</Card.Title>
            <Card.Text>Go to the Team Manager to create a team</Card.Text>
          </Card.Body>
        </Card>
        <Col md={8}>
          <CreateSection />
        </Col>
      </>
    );
  } else {
    return (
      <>
        <Menu />
        <div className="MyTeams">
          <h1
            style={{
              color: "white",
              fontSize: "2.5rem",
              fontWeight: "bold",
              textShadow: "2px 2px 4px rgba(0, 0, 0, 0.5)",
            }}
          >
            Team Manager
          </h1>
        </div>
        <Container>
          <Col md={8}>
            <CreateSection
            setTeams = {setTeams}
            loading = {loading}
            cookies = {cookies}
            />
          </Col>
          <Row>
            <Col md={10} style={{ padding: 30, marginLeft: 0 }}>
              <ManageSection />
            </Col>
          </Row>
        </Container>
      </>
    );
  }
};

export default TeamManager;
