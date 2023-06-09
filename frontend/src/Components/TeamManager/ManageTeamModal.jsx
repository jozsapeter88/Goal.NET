import { Button, Row, Col, Alert, ListGroup, Modal, Form } from "react-bootstrap";


const ManageTeamModal = ({showManageTeamModal, handleCloseManageTeamModal,selectedTeam,setShowNameModal,
    handleDeleteTeam}) => {

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
            <Button variant="success" onClick={() => setShowTeamList(true)}>
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
            {showTeamList && <TeamList />}
          </Modal.Body>
        </Modal>
    )
}
export default ManageTeamModal;
