import React, { useState } from "react";
import {
  Button,
  Row,
  Col,
  Modal,
} from "react-bootstrap";
import "./Dashboard.css";
import MyTeams from "../../Components/MyTeams";
import MatchHistory from "../../Components/MatchHistory";

const Dashboard = () => {

  const [showDetailsModal, setShowDetailsModal] = useState(false);
  const [selectedMatchDetails, setSelectedMatchDetails] = useState("");

  const handleCloseModal = () => {
    setShowDetailsModal(false);
  };

  return (
    <div>
      <div className="MyTeams">
        <h1 style={{ color: "White", marginLeft: '42vh'}}>Dashboard</h1>
      </div>
      <div className="dashboard-container">
        <Row>
          <Col sm={6}>
            <MyTeams  />
          </Col>
          <Col sm={4} className="mb-4">
            <MatchHistory />
          </Col>
        </Row>
      </div>
      <Modal
        show={showDetailsModal}
        onHide={handleCloseModal}
        size="lg"
        centered
      >
        <Modal.Header>
          <Modal.Title>Match Details</Modal.Title>
        </Modal.Header>
        <Modal.Body>{selectedMatchDetails}</Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleCloseModal}>
            Close
          </Button>
        </Modal.Footer>
      </Modal>
    </div>
  );
};

export default Dashboard;