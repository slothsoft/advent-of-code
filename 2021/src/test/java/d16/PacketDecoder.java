package d16;

/**
 * <a href="https://adventofcode.com/2021/day/16">Day 16: Packet Decoder</a>: As you leave the cave and reach open
 * waters, you receive a transmission from the Elves back on the ship.
 * <p>
 * The transmission was sent using the Buoyancy Interchange Transmission System (BITS), a method of packing numeric
 * expressions into a binary sequence. Your submarine's computer has saved the transmission in hexadecimal (your
 * puzzle input).
 */
public final class PacketDecoder {
    public PacketDecoder() {
    }

    public Packet parseHex(String hex) {
        return parseStream(BinaryStream.ofHex(hex));
    }
    public Packet parseBinary(String binary) {
        return parseStream(BinaryStream.ofBinary(binary));
    }

    private Packet parseStream(BinaryStream stream) {
        try {
            return Packet.parse(stream);
        } catch (NumberFormatException e) {
            throw new IllegalArgumentException("Error on index " + stream.getIndex() +" in input:\n" + stream.getString(), e);
        }
    }
}
