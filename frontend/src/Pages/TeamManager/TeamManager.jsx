import React, { useState, useEffect } from "react";
import { Container, Row, Col } from "react-bootstrap";
import Menu from "../../Components/Menu/Menu";
import CreateSection from "../../Components/CreateSection/CreateSection";
import ManageSection from "../../Components/ManageSection/ManageSection";
import useCookies from "react-cookie/cjs/useCookies";
import { useNavigate } from "react-router-dom/dist";
import Loading from "../../Components/Loading";
import { API_URL } from "../../Variables";

const TeamManager = () => {
  const [cookies] = useCookies();
  const [loading, setLoading] = useState(true);
  const [loadingPlayers, setLoadingPlayers] = useState(true)
  const [teams, setTeams] = useState([]);
  const [players, setPlayers] = useState([])
  console.log(players.length)
  console.log(teams.length)

  const fetchPlayers = () => {
    return fetch(`${API_URL}/players/getAllPlayers`, {
      headers: {
        Authorization: "Bearer " + cookies["token"],
      }  
    }).then((res) => res.json());
  };

  const fetchTeamsOfUser = (signal) => {
    return fetch(`${API_URL}/teams/user/getTeams`, {
      headers: {
        Authorization: "Bearer " + cookies["token"],
      },
      signal,
    }).then((res) => res.json());
  };

  useEffect(() => {
    fetchPlayers()
      .then((playersData) => {
        setPlayers(playersData)
        setLoadingPlayers(false)
      })
      .catch((error) => {
        if (error.name !== "AbortError") {
          setPlayers([]);
          throw error;
        }
      });

  }, []);

  useEffect(() => {
    const controller = new AbortController();
    fetchTeamsOfUser()
      .then((teamsData) => {
        setTeams(teamsData);
        setLoading(false);
      })
    fetchPlayers()
      .then((playersData) => {
        setPlayers(playersData)
        setLoading(false)
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
  
  if (cookies["token"] === undefined || cookies["username"] === undefined) {
    navigate("/");
  }

  if (loading || loadingPlayers) {
    return <Loading />;
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
          <Col md={10} style={{ padding: 30, marginLeft: 50 }}>
            <ManageSection
            teams={teams}
            setTeams={setTeams}
            loading={loading}
            players={players}
            loadingPlayers={loadingPlayers}
            setLoadingPlayers={setLoadingPlayers} />
          </Col>
        </Row>
      </Container>
    </>
  );
};

export default TeamManager;
