import { minecraftColors } from "../../minecraftFormatting.js";

const formatRichMotd = (motdEntries, end) => {
    let strMotd = "";

    motdEntries.forEach(entry => {
        if (entry.bold) strMotd += "§1";
        if (entry.italic) strMotd += "§o";
        if (entry.underlined) strMotd += "§n";
        if (entry.obfuscated) strMotd += "§k";
        
        if (entry.color) {
            // check if color is a hex code, because some servers are stupid this way
            if (/^#[0-9A-F]{6}$/i.test(`#${entry.color.replace("#", "")}`)) {
                Object.entries(minecraftColors).forEach(((colorEntry) => {
                    const [color, data] = colorEntry;

                    if (data.hex === entry.color.replace("#", "")) {
                        strMotd += "§" + data.code;
                    }
                }));
            } else { // color field is something like dark_red
                strMotd += "§" + minecraftColors[entry.color].code;
            }
        }

        strMotd += entry.text || "";
    });

    return strMotd + (end || "");
}

export const stringifyMotd = (motd) => {
    if (typeof(motd) === "object") {

    }
}