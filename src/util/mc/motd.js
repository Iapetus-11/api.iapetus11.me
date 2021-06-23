import { minecraftColors, minecraftColorsCodes } from "../../minecraftFormatting.js";

const formatRichMotd = (motdEntries, end) => {
  let strMotd = "";

  motdEntries.forEach((entry) => {
    if (entry.bold) strMotd += "§l";
    if (entry.italic) strMotd += "§o";
    if (entry.underlined) strMotd += "§n";
    if (entry.obfuscated) strMotd += "§k";

    if (entry.color) {
      // check if color is a hex code, because some servers are stupid this way
      if (/^#[0-9A-F]{6}$/i.test(`#${entry.color.replace("#", "")}`)) {
        Object.entries(minecraftColors).forEach((colorEntry) => {
          const [color, data] = colorEntry;

          if (data.hex === entry.color.replace("#", "")) {
            strMotd += "§" + data.code;
          }
        });
      } else {
        // color field is something like dark_red
        strMotd += "§" + minecraftColors[entry.color].code;
      }
    }

    strMotd += entry.text || "";
  });

  return strMotd + (end || "");
};

export const stringifyMotd = (motd) => {
  if (Array.isArray(motd)) {
    return formatRichMotd(motd);
  } else if (typeof motd === "object") {
    return formatRichMotd(motd.extra, motd.text);
  } else {
    return `${motd}`;
  }
};

export const parseColors = (strMotd) => {
  let rich = [];
  let current = {text: "", color: "#FFFFFF"};

  for (let i = 0; i < strMotd.length; i++) {
    const c = strMotd[i];

    if (c === "§") {
      let color;

      try {
        color = "#" + minecraftColorsCodes[strMotd[i+1]].hex;
      } catch (e) {
        i += 1;
        continue;
      }

      if (color !== current.color) {
        rich.push({...current});

        current.color = color;
        current.text = "";
      }
      
      i += 1;
    } else {
        current.text += c;
    }
  }

  rich.push({...current});

  return rich;
}

// console.log(parseColors(formatRichMotd([
//     {"text": "brh"},
//     {"text": "two", "color": "aqua", "bold": true},
//     {"text": "three", "italic": false, "e": 3},
//     {"text": "two", "color": "#FFFFFF", "italic": true},
// ])));

console.log(parseColors(formatRichMotd([
    {"text": "white"},
    {"text": "aqua", "color": "aqua", "bold": true},
    {"text": "aqua", "italic": true},
    {"text": "yellow", "color": "yellow", "italic": false},
    {"text": "yellow", "color": "yellow", "bold": true},
])));