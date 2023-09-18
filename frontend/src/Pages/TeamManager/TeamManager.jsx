import React, { useState, useEffect, createContext, useContext } from "react";
import { Container, Row, Col, Button, Modal, Form } from "react-bootstrap";
import Menu from "../../Components/Menu/Menu";
import CreateSection from "../../Components/CreateSection/CreateSection";
import ManageSection from "../../Components/ManageSection/ManageSection";
import useCookies from "react-cookie/cjs/useCookies";
import { useNavigate } from "react-router-dom/dist";
import Loading from "../../Components/Loading";
import { UserContext } from "../../Contexts/UserContext";
import AddPlayerList from "../../Components/AddPlayerList/AddPlayerList";
import ConfirmationModal from "../../Components/CofirmationModal";

export const fetchCurrentUser = (cookies) => {
  return fetch(process.env.REACT_APP_API_URL + `/user/currentUser`, {
    headers: {
      Authorization: "Bearer " + cookies["token"],
    },
  }).then((res) => res.json());
};

const fetchPlayers = (cookies) => {
  return fetch(process.env.REACT_APP_API_URL + `/players/getAllPlayers`, {
    headers: {
      Authorization: "Bearer " + cookies["token"],
    },
  }).then((res) => res.json());
};

const fetchTeamsOfUser = (cookies) => {
  return fetch(process.env.REACT_APP_API_URL + `/teams/user/getTeams`, {
    headers: {
      Authorization: "Bearer " + cookies["token"],
    },
  }).then((res) => res.json());
};

const buyPlayerByUser = async (cookies, teamId, playerId) => {
  return await fetch(
    process.env.REACT_APP_API_URL +
      `/teams/addPlayerToTeam/${teamId}/${playerId}`,
    {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + cookies["token"],
      },
    }
  );
};

export const fetchPlayersOfTeam = async (cookies, teamId) => {
  console.log(teamId);
  return await fetch(
    process.env.REACT_APP_API_URL + `/teams/getPlayersOfTeam/${teamId}`,
    {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + cookies["token"],
      },
    }
  );
};

export const PlayerContext = createContext("player");
const TeamManager = () => {
  const [cookies] = useCookies();
  const [loading, setLoading] = useState(true);
  const [loadingPlayers, setLoadingPlayers] = useState(true);
  const [teams, setTeams] = useState([]);
  const [showPlayerList, setShowPlayerList] = useState(false);
  const [isBuyOpen, setIsBuyOpen] = useState(false);
  const [playerId, setPlayerId] = useState(null);
  const [teamId, setTeamId] = useState(null);
  const { user, setUser } = useContext(UserContext);
  const [players, setPlayers] = useState([]);
  const [showMyPlayers, setShowMyPlayers] = useState(false);
  const [teamPlayers, setTeamPlayers] = useState([]);
  const [showCreateTeamModal, setShowCreateTeamModal] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    fetchTeamsOfUser(cookies).then((teamsData) => {
      setTeams(teamsData);
      setLoading(false);
    });
    fetchPlayers(cookies)
      .then((playersData) => {
        setPlayers(playersData);
        setLoadingPlayers(false);
      })
      .catch((error) => {
        if (error.name !== "AbortError") {
          setTeams([]);
          throw error;
        }
      });
  }, []);

  const onClickMyPlayers = async (team) => {
    const players = await fetchPlayersOfTeam(cookies, team.id);
    if (players.status === 200) {
      const data = await players.json();
      console.log("data", data);
      setTeamPlayers(data);
    } else if (players.status === 404) {
      console.log("no player");
      alert("No players yet!");
    }
  };

  const handleCreateTeamModal = () => {
    setShowCreateTeamModal(!showCreateTeamModal);
  };

  const onConfirm = async () => {
    const buy = await buyPlayerByUser(cookies, teamId, playerId);
    console.log("status: " + buy.status);
    if (buy.status === 200) {
      const data = await buy.json();
      const updatedUser = { ...user, points: data.userPoint };
      console.log(updatedUser);
      console.log("points: " + data.userPoint);
      // Update the 'user' item in localStorage
      localStorage.setItem("user", JSON.stringify(updatedUser));
      // Update the user context with the new user object
      setUser(updatedUser);
      alert("Player is successfully purchased!");
    } else if (buy.status === 400) {
      alert("You can't afford this player or you already have it!");
    } else if (buy.status === 404) {
      alert("This team doesn't exist ");
    }
    setIsBuyOpen(!isBuyOpen);
  };

  if (cookies["token"] === undefined || cookies["username"] === undefined) {
    navigate("/");
  }

  if (loading || loadingPlayers) {
    return <Loading />;
  }
  return (
    <>
      <PlayerContext.Provider value={{ players, setPlayers }}>
        <Menu />
        <div className="teammanager-title">
          <h1
            style={{
              color: "white",
            }}
          >
            Available points:
          </h1>
          <h1
            style={{
              color: "white",
              fontSize: "2.5rem",
              fontWeight: "bold",
              textShadow: "2px 2px 4px rgba(0, 0, 0, 0.5)",
            }}
          >
            {user.points}
          </h1>
        </div>
        <Button onClick={handleCreateTeamModal}>Create a team</Button>

        <CreateSection setTeams={setTeams} />

        <ManageSection
          teams={teams}
          setTeams={setTeams}
          loading={loading}
          loadingPlayers={loadingPlayers}
          setLoadingPlayers={setLoadingPlayers}
          showPlayerList={showPlayerList}
          setShowPlayerList={setShowPlayerList}
          setTeamId={setTeamId}
          setShowMyPlayers={setShowMyPlayers}
          showMyPlayers={showMyPlayers}
          onClickMyPlayers={onClickMyPlayers}
        />

        {showPlayerList && (
          <AddPlayerList
            loadingPlayers={loadingPlayers}
            setIsBuyOpen={setIsBuyOpen}
            setPlayerId={setPlayerId}
            players={players}
          />
        )}

        {showMyPlayers && (
          <AddPlayerList
            loadingPlayers={loadingPlayers}
            showMyPlayers={showMyPlayers}
            setShowMyPlayers={setShowMyPlayers}
            setPlayerId={setPlayerId}
            players={teamPlayers}
          />
        )}

        {isBuyOpen && (
          <ConfirmationModal
            isBuyOpen={isBuyOpen}
            setIsBuyOpen={setIsBuyOpen}
            onConfirm={onConfirm}
          />
        )}
      </PlayerContext.Provider>
      <Modal show={showCreateTeamModal} onHide={handleCreateTeamModal}>
        <Modal.Header closeButton>
          <Modal.Title>Create a Team</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form>
            <Form.Group controlId="teamName">
              <Form.Label>Name</Form.Label>
              <Form.Control type="text" placeholder="Enter team name" />
            </Form.Group>
            <Form.Group controlId="teamColor">
              <Form.Label>Color</Form.Label>
              <Form.Control type="text" placeholder="Enter team color" />
            </Form.Group>
          </Form>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="primary" onClick={handleCreateTeamModal}>
            Create Team
          </Button>
          <Button variant="secondary" onClick={handleCreateTeamModal}>
            Cancel
          </Button>
        </Modal.Footer>
      </Modal>
    </>
  );
};

export default TeamManager;
