import React from "react";
import { Container, Row, Col, Table } from "react-bootstrap";

const MatchResult = ({ date, team1, team2, score1, score2, logo1, logo2 }) => {
  const timeString = new Date(date).toLocaleTimeString([], {
    hour: "2-digit",
    minute: "2-digit",
    hour12: false,
  });
  return (
    <tr>
      <td>{timeString}</td>
      <td>
        <img
          src={logo1}
          alt={`${team1} Logo`}
          style={{ width: "50px", height: "50px" }}
        />
      </td>
      <td>{team1}</td>
      <td>{team2}</td>
      <td>{score1}</td>
      <td>{score2}</td>
      <td>
        <img
          src={logo2}
          alt={`${team2} Logo`}
          style={{ width: "50px", height: "50px" }}
        />
      </td>
    </tr>
  );
};

const LeagueResults = ({ leagueName, matchResults }) => {
  return (
    <div className="my-5">
      <h2>{leagueName}</h2>
      <Table striped bordered hover>
        <thead>
          <tr>
            <th>Date</th>
            <th></th>
            <th>Team 1</th>
            <th>Team 2</th>
            <th>Score 1</th>
            <th>Score 2</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {matchResults.map((matchResult, index) => (
            <MatchResult key={index} {...matchResult} />
          ))}
        </tbody>
      </Table>
    </div>
  );
};

const Matches = () => {
  const leagues = [
    {
      leagueName: "Premier League",
      matchResults: [
        {
          date: "2023-05-01T18:30:00",
          team1: "Manchester United",
          logo1: "https://i.imgur.com/i6kJ8hv.png",
          team2: "Liverpool",
          logo2: "https://i.imgur.com/nBzzT0C.png",
          score1: 2,
          score2: 1,
        },
        {
          date: "2023-05-02T19:00:00",
          team1: "Chelsea",
          logo1: "https://i.imgur.com/TaCgLjz.png",
          team2: "Arsenal",
          logo2: "https://i.imgur.com/YZVSA7d.png",
          score1: 1,
          score2: 1,
        },
      ],
    },
    {
      leagueName: "La Liga",
      matchResults: [
        {
          date: "2023-05-01T16:00:00",
          team1: "Barcelona",
          logo1: "https://i.imgur.com/eoP5VCP.png",
          team2: "Real Madrid",
          logo2: "https://i.imgur.com/NxL8ys7.png",
          score1: 2,
          score2: 2,
        },
        {
          date: "2023-05-02T18:30:00",
          team1: "Atletico Madrid",
          logo1: "https://i.imgur.com/YxJQzij.png",
          team2: "Sevilla",
          logo2: "https://i.imgur.com/Rp9Jbim.png",
          score1: 1,
          score2: 0,
        },
      ],
    },
  ];

  return (
    <Container
      className="d-flex flex-column align-items-center"
      style={{ maxWidth: "800px" }}
    >
      <Row>
        <Col>
          {leagues.map((league, index) => (
            <LeagueResults key={index} {...league} />
          ))}
        </Col>
      </Row>
    </Container>
  );
};

export default Matches;