import React, { useState, useEffect } from "react";
import { Container, Row, Col } from "react-bootstrap";
import Menu from "../Menu/Menu";
import CreateSection from "./CreateSection";
import ManageSection from "./ManageSection";
import Cookies from "universal-cookie";
import { useCookies } from "react-cookie";
import { useNavigate } from "react-router-dom/dist";

const TeamManager = () => {
  const [loading, setLoading] = useState(true);
  const [teams, setTeams] = useState([]);

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
    fetchTeamsOfUser()
      .then((teamsData) => {
        setTeams(teamsData);
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

  const navigate = useNavigate();
  const [cookies] = useCookies();
  if (cookies["token"] === undefined || cookies["username"] === undefined) {
    navigate("/");
  }
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
        <Row>
          <Col md={1}>
            <CreateSection
            setTeams = { setTeams }
            />
          </Col>
          <Col md={10} style={{ padding: 30, marginLeft: 0 }}>
            <ManageSection />
          </Col>
        </Row>
      </Container>
    </>
  );
};

export default TeamManager;
