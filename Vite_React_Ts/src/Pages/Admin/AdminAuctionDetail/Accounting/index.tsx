import React from "react";
import { Card } from "antd";
import { Typography } from "@material-tailwind/react";
import { NumberFormat } from "../../../../utils/numbetFormat";

const Accounting: React.FC = () => (
  <Card title="Auction accounting" bordered={false}>
    <div className="flex">
      <div>
        <Typography>Real Estate</Typography>
        <Typography>Winner name</Typography>
        <Typography>Deposit amount</Typography>
        <Typography>Max amount</Typography>
        <Typography>Commission amount</Typography>
        <Typography>Owner name</Typography>
        <Typography>Owner receive</Typography>
      </div>
      <div className="px-10">
        <Typography>Rest demo</Typography>
        <Typography>Winner 1</Typography>
        <Typography>{NumberFormat(10000000)}</Typography>
        <Typography>{NumberFormat(100000000)}</Typography>
        <Typography>{NumberFormat(2000000)}</Typography>
        <Typography>Owner 1</Typography>
        <Typography>{NumberFormat(10000000)}</Typography>
      </div>
    </div>
  </Card>
);

export default Accounting;
