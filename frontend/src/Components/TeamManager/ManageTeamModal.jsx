import { Button, Modal } from "react-bootstrap";
import TeamList from "../TeamList/TeamList";
import { useState } from "react";


const ManageTeamModal = ({showManageTeamModal, handleCloseManageTeamModal,selectedTeam,setShowNameModal,
    handleDeleteTeam,showTeamList, setShowTeamList, players, loadingPlayers}) => {

       

    return (
      <Modal
          show={showManageTeamModal}
          onHide={handleCloseManageTeamModal}
          style={{ background: "rgba(0,0,0, 0.7)" }}
        >
          <Modal.Header>
            <Modal.Title>Manage Team: {selectedTeam.name}</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <Button variant="success" onClick={() => setShowTeamList(true)}>Add Player
                </Button>
            <Button 
              variant="warning" 
              onClick={(e) => setShowNameModal(true)}>
                Edit Name
                </Button>
            <Button
              variant="danger"
              onClick={() => handleDeleteTeam(selectedTeam.id)}
            >
              Delete
            </Button>
            {showTeamList && <TeamList 
            players={players}
            loadingPlayers={loadingPlayers}/>}
          </Modal.Body>
        </Modal>
    )
}
export default ManageTeamModal;
