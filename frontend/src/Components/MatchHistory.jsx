/*-------------------This component is used in the Dashboard-------------------*/

import React from "react";
import { Card, Table } from "react-bootstrap";

export default function MatchHistory() {
  const handleMatchClick = (matchId, matchDetails) => {
    setExpandedMatchId((prevMatchId) =>
      prevMatchId === matchId ? null : matchId
    );
    setSelectedMatchDetails(matchDetails);
    setShowDetailsModal(true);
  };

  /*-------------------Hardcoded match history data-------------------*/
  const matchHistory = [
    {
      id: 1,
      team1: "Real Madrid",
      team2: "Barcelona",
      score: "2-1",
      details: "Match details for Real Madrid vs Barcelona...",
    },
    {
      id: 2,
      team1: "Liverpool",
      team2: "Manchester City",
      score: "0-0",
      details: "Match details for Liverpool vs Manchester City...",
    },
    {
      id: 3,
      team1: "Bayern Munich",
      team2: "Paris Saint-Germain",
      score: "3-2",
      details: "Match details for Bayern Munich vs Paris Saint-Germain...",
    },
    {
      id: 4,
      team1: "Manchester United",
      team2: "Chelsea",
      score: "1-1",
      details: "Match details for Manchester United vs Chelsea...",
    },
    {
      id: 5,
      team1: "Juventus",
      team2: "AC Milan",
      score: "2-0",
      details: "Match details for Juventus vs AC Milan...",
    },
    {
      id: 6,
      team1: "Borussia Dortmund",
      team2: "RB Leipzig",
      score: "2-2",
      details: "Match details for Borussia Dortmund vs RB Leipzig...",
    },
    {
      id: 7,
      team1: "Arsenal",
      team2: "Tottenham Hotspur",
      score: "1-0",
      details: "Match details for Arsenal vs Tottenham Hotspur...",
    },
    {
      id: 8,
      team1: "Inter Milan",
      team2: "Lazio",
      score: "3-1",
      details: "Match details for Inter Milan vs Lazio...",
    },
    {
      id: 9,
      team1: "Atletico Madrid",
      team2: "Sevilla",
      score: "2-2",
      details: "Match details for Atletico Madrid vs Sevilla...",
    },
    {
      id: 10,
      team1: "Ajax",
      team2: "PSV Eindhoven",
      score: "2-1",
      details: "Match details for Ajax vs PSV Eindhoven...",
    },
    /*-------------------Hardcoded match history data-------------------*/
  ];

  return (
    <Card bg="dark" text="white">
      <Card.Body>
        <h4>Previous Match History</h4>
        <Table striped bordered hover variant="dark">
          <thead>
            <tr>
              <th>Team 1</th>
              <th>Team 2</th>
              <th>Score</th>
              <th>Date</th>
            </tr>
          </thead>
          <tbody>
            {matchHistory.map((match) => (
              <React.Fragment key={match.id}>
                <tr onClick={() => handleMatchClick(match.id, match.details)}>
                  <td>{match.team1}</td>
                  <td>{match.team2}</td>
                  <td>{match.score}</td>
                  <td></td>
                </tr>
              </React.Fragment>
            ))}
          </tbody>
        </Table>
      </Card.Body>
    </Card>
  );
}
