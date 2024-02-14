import { Collapse } from "antd";
import Auction from "./Auction";

interface Props {
  id: string | undefined;
}

const Summary: React.FC<Props> = ({ id }) => {
  let AuctionId = id;
  return (
    <>
      <Collapse
        collapsible="header"
        defaultActiveKey={["1"]}
        items={[
          {
            key: "1",
            label: "Auction",
            children: <Auction id={AuctionId} />,
          },
        ]}
      />
      <Collapse
        collapsible="header"
        defaultActiveKey={["1"]}
        items={[
          {
            key: "1",
            label: "Note",
            children: <p>type something</p>,
          },
        ]}
      />
    </>
  );
};

export default Summary;
