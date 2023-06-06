import { Container, Row, Col } from "react-bootstrap";
import Menu from "../Menu/Menu";
import CreateSection from "./CreateSection";
import ManageSection from "./ManageSection";
import Cookie from "universal-cookie";
const TeamManager = () => {
  return (
    <>
      <Menu />
      <div className="MyTeams">
      <h1 style={{ color: "white", fontSize: "2.5rem", fontWeight: "bold", textShadow: "2px 2px 4px rgba(0, 0, 0, 0.5)" }}>Team Manager</h1>
      </div>
      <Container>
        <Row>
          <Col md={1}>
            <CreateSection />
          </Col>
          <Col md={10} style={{padding: 30, marginLeft: 0}}>
            <ManageSection />
          </Col>
        </Row>
      </Container>
    </>
  );
};

export default TeamManager;
