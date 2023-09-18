/*-------------------This component is used in the TeamManager-------------------*/

import { Button, Modal } from "react-bootstrap";
import AddPlayerList from "./AddPlayerList/AddPlayerList";

const ManageTeamModal = ({
  showManageTeamModal,
  handleCloseManageTeamModal,
  selectedTeam,
  setShowNameModal,
  handleDeleteTeam,
  showPlayerList,
  setShowPlayerList,
  setShowManageTeamModal,
  loadingPlayers,
}) => {
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
        <Button variant="warning" onClick={(e) => setShowNameModal(true)}>
          Edit Name
        </Button>
        <Button
          variant="danger"
          onClick={() => handleDeleteTeam(selectedTeam.id)}
        >
          Delete
        </Button>
        {showPlayerList && (
          <AddPlayerList 
           loadingPlayers={loadingPlayers}
           setShowManageTeamModal={setShowManageTeamModal} />
        )}
        <Button variant="primary" onClick={() => handleCloseManageTeamModal()}>
          Close
        </Button>
      </Modal.Body>
    </Modal>
  );
};
export default ManageTeamModal;
