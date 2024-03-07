import { Pie } from "@ant-design/plots";

const PropertiesPie: React.FC = () => {
  const data = [
    {
      type: "Residential",
      value: 27,
    },
    {
      type: "Commercial",
      value: 25,
    },
    {
      type: "Land",
      value: 18,
    },
    {
      type: "Investment",
      value: 15,
    },
    {
      type: "Farmland",
      value: 10,
    },
    {
      type: "Special Purpose",
      value: 5,
    },
  ];
  const config = {
    appendPadding: 10,
    data,
    angleField: "value",
    colorField: "type",
    radius: 0.8,
    label: {
      type: "outer",
    },
    interactions: [
      {
        type: "element-active",
      },
    ],
  };
  return <Pie {...config} />;
};

export default PropertiesPie;
