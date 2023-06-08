import { Button, Row, Col, Alert, ListGroup, Modal, Form } from "react-bootstrap";


const ManageTeamModal = ({showManageTeamModal, handleCloseManageTeamModal,selectedTeam,setShowNameModal,
    handleDeleteTeam}) => {

    return (
        <Modal
          show={showManageTeamModal}
          onHide={handleCloseManageTeamModal}
          variant="dark"
        >
          <Modal.Header>
            <Modal.Title>Manage Team: {selectedTeam.name}</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <Button variant="success">Add a Player</Button>{" "}
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
          </Modal.Body>
        </Modal>
    )
}
export default ManageTeamModal;