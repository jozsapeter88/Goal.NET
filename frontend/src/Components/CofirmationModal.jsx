import React from 'react';
import { Button, Modal } from "react-bootstrap";

function ConfirmationModal({ isBuyOpen,setIsBuyOpen, onRequestClose, onConfirm }) {
    return (
      <Modal
        show={isBuyOpen}
        //onRequestClose={onRequestClose}
        //style={customStyles}
       
      >
        <h2>Are you sure you want to buy this player?</h2>
        <Button onClick={onConfirm}>Yes</Button>
        <Button onClick={(e) => setIsBuyOpen(false)}>No</Button>
      </Modal>
    );
  }
  
  export default ConfirmationModal;