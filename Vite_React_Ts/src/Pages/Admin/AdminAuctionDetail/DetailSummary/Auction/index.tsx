import { Typography } from "@material-tailwind/react";
import dayjs from "dayjs";

interface Props {
  id: string | undefined;
}
const getRandomInt = (min: number, max: number) => {
  return Math.floor(Math.random() * (max - min + 1)) + min;
};
const generateRandomDate = () => {
  const startDate = dayjs().subtract(10, "year");
  const endDate = dayjs();
  const randomTimestamp = getRandomInt(startDate.valueOf(), endDate.valueOf());
  return dayjs(randomTimestamp);
};
const Auction: React.FC<Props> = ({ id }) => {
  return (
    <>
      <div className="flex">
        <div>
          <Typography>Auction id</Typography>
          <Typography>Auction name</Typography>
          <Typography>Auction date</Typography>
          <Typography>Auction start time</Typography>
          <Typography>Created by</Typography>
          <Typography>Status</Typography>
          <Typography>Total bidders</Typography>
        </div>
        <div className="px-10">
          <Typography>{id?.toString()}</Typography>
          <Typography>Reas demo</Typography>
          <Typography>{generateRandomDate().format("YYYY-MM-DD")}</Typography>
          <Typography>11:00 AM</Typography>
          <Typography>Staff 1</Typography>
          <Typography>Completed</Typography>
          <Typography>20</Typography>
        </div>
      </div>
    </>
  );
};

export default Auction;
