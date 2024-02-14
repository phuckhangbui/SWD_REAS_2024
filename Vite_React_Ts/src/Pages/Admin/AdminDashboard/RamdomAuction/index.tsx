import { Card } from "antd";
import AuctionCard from "../AuctionCard";

const gridStyle: React.CSSProperties = {
  width: "33.333%",
  textAlign: "center",
};

const RandomAuction: React.FC = () => {
  return (
    <>
      <Card
        title={
          <div className="flex justify-between">
            <span
              style={{
                color: "#bdc3c9",
                fontSize: "30px",
                fontWeight: "bold",
              }}
            >
              Auction
            </span>
            <span
              style={{
                color: "#bdc3c9",
                fontSize: "30px",
                fontWeight: "bold",
              }}
            >
              Total Auction: {<span className="text-black">30</span>}
            </span>
            <span
              style={{
                color: "#477ffb",
                fontSize: "30px",
                fontWeight: "bold",
              }}
            >
              See all
            </span>
          </div>
        }
      >
        <Card.Grid style={gridStyle} hoverable={false}>
          <AuctionCard />
        </Card.Grid>
        <Card.Grid style={gridStyle} hoverable={false}>
          <AuctionCard />
        </Card.Grid>
        <Card.Grid style={gridStyle} hoverable={false}>
          <AuctionCard />
        </Card.Grid>
      </Card>
    </>
  );
};

export default RandomAuction;
