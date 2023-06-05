import Menu from "../Menu/Menu";
import CreateSection from "./CreateSection";
import ManageSection from "./ManageSection";

const HomePage = () => {
  return (
    <div>
      <Menu />
      <CreateSection />
      <ManageSection />
    </div>
  );
};

export default HomePage;
