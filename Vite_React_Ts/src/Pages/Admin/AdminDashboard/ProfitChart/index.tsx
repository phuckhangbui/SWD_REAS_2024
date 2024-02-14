import { Line } from "@ant-design/plots";

const DemoLine: React.FC = () => {
  const data = [
    {
      year: "Jan",
      value: 3,
    },
    {
      year: "Feb",
      value: 4,
    },
    {
      year: "Mar",
      value: 3.5,
    },
    {
      year: "Apr",
      value: 5,
    },
    {
      year: "May",
      value: 4.9,
    },
    {
      year: "Jun",
      value: 6,
    },
    {
      year: "Jul",
      value: 7,
    },
    {
      year: "Aug",
      value: 9,
    },
    {
      year: "Sep",
      value: 13,
    },
    {
      year: "Oct",
      value: 15,
    },
    {
      year: "Nov",
      value: 12,
    },
    {
      year: "Dev",
      value: 18,
    },
  ];
  const config = {
    data,
    xField: "year",
    yField: "value",
    label: {},
    point: {
      size: 5,
      shape: "diamond",
      style: {
        fill: "white",
        stroke: "#5B8FF9",
        lineWidth: 2,
      },
    },
    tooltip: {
      showMarkers: false,
    },
    state: {
      active: {
        style: {
          shadowBlur: 4,
          stroke: "#000",
          fill: "red",
        },
      },
    },
    interactions: [
      {
        type: "marker-active",
      },
    ],
  };
  return <Line {...config} />;
};

export default DemoLine;
