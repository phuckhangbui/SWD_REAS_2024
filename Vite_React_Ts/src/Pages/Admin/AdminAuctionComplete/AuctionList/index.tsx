import { MagnifyingGlassIcon } from "@heroicons/react/20/solid";
import { Avatar, Input, Typography } from "@material-tailwind/react";
import { Table, TableProps } from "antd";
import dayjs, { Dayjs } from "dayjs";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
interface CompletedAuction {
  key: number;
  reas: string;
  date: Dayjs;
  minimumIncrement: number;
  winner: string;
  imgWinner: string;
  basePrice: number;
  finalPrice: number;
}
const CompleteList: React.FC = () => {
  const [search, setSearch] = useState("");
  const navigate = useNavigate();
  const viewDetail = (key: number) => {
    navigate(`/admin/auction/detail/${key}`);
  };
  const columns: TableProps<CompletedAuction>["columns"] = [
    {
      title: "Id",
      dataIndex: "key",
      width: "5%",
      render: (text) => <a>{text}</a>,
    },
    {
      title: "Real Estate",
      dataIndex: "reas",
      width: "15%",
    },
    {
      title: "Date",
      dataIndex: "date",
      render: (date: Dayjs) => (
        <Typography>{date.format("YYYY-MM-DD")}</Typography>
      ), // Render the date as a string
      width: "10%",
    },
    {
      title: "Minimum Increment",
      dataIndex: "minimumIncrement",
      width: "10%",
    },
    {
      title: "Winner",
      dataIndex: "winner",
      render: (_: any, record: CompletedAuction) => (
        <div className="flex items-center gap-3">
          <Avatar src={record.imgWinner} alt={record.winner} size="sm" />
          <div className="flex flex-col">
            <Typography color="blue-gray" className="font-normal">
              {record.winner}
            </Typography>
          </div>
        </div>
      ),
      width: "15%",
    },
    {
      title: "Base Price",
      dataIndex: "basePrice",
      width: "10%",
    },
    {
      title: "Final Price",
      dataIndex: "finalPrice",
      width: "10%",
    },
    {
      title: "",
      dataIndex: "operation",
      render: (_: any, record: CompletedAuction) => (
        <a onClick={() => viewDetail(record.key)}>View details</a>
      ),
      width: "10%",
    },
  ];

  const getRandomInt = (min: number, max: number) => {
    return Math.floor(Math.random() * (max - min + 1)) + min;
  };

  // Generate random dates within a range of 10 years from today
  const generateRandomDate = () => {
    const startDate = dayjs().subtract(10, "year");
    const endDate = dayjs();
    const randomTimestamp = getRandomInt(
      startDate.valueOf(),
      endDate.valueOf()
    );
    return dayjs(randomTimestamp);
  };

  // Generate 100 random CompletedAuction items
  const generateRandomCompletedAuctions = (): CompletedAuction[] => {
    const items: CompletedAuction[] = [];
    for (let i = 1; i <= 100; i++) {
      const basePrice = getRandomInt(1000, 10000);
      const finalPrice = getRandomInt(basePrice, basePrice + 10000); // Ensure final price is larger than base price
      const auction: CompletedAuction = {
        key: i,
        reas: `Item ${i}`,
        date: generateRandomDate(),
        minimumIncrement: getRandomInt(100, 500),
        winner: `Winner ${i}`,
        imgWinner: `https://example.com/winner_${i}.jpg`,
        basePrice,
        finalPrice,
      };
      items.push(auction);
    }
    return items;
  };

  const data: CompletedAuction[] = generateRandomCompletedAuctions();

  return (
    <>
      <div className="flex flex-col items-center justify-between gap-4 md:flex-row">
        <div className="w-full md:w-72 flex flex-row justify-start">
          {/* <Select
            defaultValue="Residential"
            onChange={handleChange}
            options={[
              { value: "residential", label: "Residential" },
              { value: "commercial", label: "Commercial" },
              { value: "land", label: "Land" },
              { value: "investment", label: "Investment" },
              { value: "farmland", label: "Farmland" },
              { value: "specialPurpose", label: "Special Purpose" },
              { value: "department", label: "Department" },
            ]}
          /> */}
        </div>

        <div className="w-full md:w-72 flex flex-row justify-end">
          <div className="flex flex-row space-between space-x-2">
            <Input
              label="Search"
              icon={<MagnifyingGlassIcon className="h-5 w-5" />}
              crossOrigin={undefined}
              onChange={(e) => setSearch(e.target.value)}
            />
          </div>
        </div>
      </div>
      <Table
        columns={columns}
        dataSource={data.filter((reas) => {
          const isMatchingSearch =
            search.toLowerCase() === "" ||
            reas.reas.toLowerCase().includes(search);

          return isMatchingSearch;
        })}
        bordered
      />
    </>
  );
};

export default CompleteList;
