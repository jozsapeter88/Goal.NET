import React from "react";
import "./TeamList.css";
import Loading from "../Loading";


const TeamList = ({players, loadingPlayers}) => {
  if (loadingPlayers) {
    return <Loading />;
  }
  return (
    <>
    
    <div className="gallery">
    {players?players.map((p) => (
      <div className="teamCard" key={p.id}>
        <h3 className="teamCard-title">Card</h3>
        <p className="teamCard-description">
          
          <p>{p.name}</p>
          
          <p>{p.nationality}</p>
          
          <p>{p.score}</p>
        </p>
      </div>
       )): <div><p>No Players</p></div>}
      </div>
      
   </>)
};

export default TeamList;
