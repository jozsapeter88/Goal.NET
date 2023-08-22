import { Button, Modal } from "react-bootstrap";
import AddPlayerList from "../AddPlayerList/AddPlayerList";
import { useState } from "react";


const ManageTeamModal = ({showManageTeamModal, handleCloseManageTeamModal,selectedTeam,setShowNameModal,
    handleDeleteTeam,showTeamList, setShowTeamList, players, loadingPlayers}) => {

       

    return (
      <Modal
          show={showManageTeamModal}
          onHide={handleCloseManageTeamModal}
          style={{ background: "rgba(0,0,0, 0.7)" }}
          backdrop="static"
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
            {showTeamList && <AddPlayerList 
            players={players}
            loadingPlayers={loadingPlayers}/>}
            <Button
              variant="primary"
              onClick={() => handleCloseManageTeamModal()}
            >
              Close
            </Button>
          </Modal.Body>
        </Modal>
    )
}
export default ManageTeamModal;
