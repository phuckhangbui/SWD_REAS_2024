import { Tabs } from "antd";
import { useParams } from "react-router-dom";
import Summary from "./DetailSummary";
import { CarouselCustomNavigation } from "./Images";
import ListBidders from "./ListBidders";
import Accounting from "./Accounting";

const AuctionDetail = () => {
  // Get the key parameter from the route
  const { key } = useParams();
  let id = key?.toString();

  const items = [
    { label: "Summary", key: "1", children: <Summary id={id} /> },
    { label: "Bidders", key: "2", children: <ListBidders /> },
    { label: "Images", key: "3", children: <CarouselCustomNavigation /> },
    { label: "Accounting", key: "4", children: <Accounting /> },
    // { label: "Real estate", key: "5", children: "Test" },
  ];
  return (
    <>
      <Tabs defaultActiveKey="1" centered size={"large"} items={items} />
    </>
  );
};

export default AuctionDetail;
