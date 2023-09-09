import React, { useContext, useState } from "react";
import "./AddPlayerList.css";
import Loading from "../Loading";
import { Button } from "react-bootstrap";
import { PlayerContext } from "../../Pages/TeamManager/TeamManager";
import ConfirmationModal from "../CofirmationModal";

const AddPlayerList = ({loadingPlayers, setIsBuyOpen, setPlayerId }) => {

  const{players} = useContext(PlayerContext)
 
  const handleBuy = (playerId)=> {
    console.log(playerId)
    setIsBuyOpen(true)
    setPlayerId(playerId)
  }
  
  if (loadingPlayers) {
    return <Loading />;
  }
  return (
    <>
      <div className="gallery">
        {players ? (
          players.map((p) => (
            <div className="teamCard" key={p.id}>
              <h3 className="teamCard-title">Card</h3>
              <div className="teamCard-description">
                <p>{p.name}</p>

                <p>{p.nationality}</p>

                <p>{p.score}</p>
              </div>
              <Button variant="warning" onClick={(e) => handleBuy(p.id)}>
                Buy
              </Button>
            </div>
          ))
        ) : (
          <div>
            <p>No Players</p>
          </div>
        )}
      </div>

    </>
  );
};

export default AddPlayerList;
