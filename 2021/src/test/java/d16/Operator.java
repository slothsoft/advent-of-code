package d16;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public abstract class Operator implements Packet {

    private final int version;
    private final int typeId;
    private final boolean lengthTypeId;
    private final int subPacketLength;
    private final Packet[] subPackets;

    public Operator(BinaryStream stream) {
        // 00111000000000000110111101000101001010010001001000000000
        // VVVTTTILLLLLLLLLLLLLLLAAAAAAAAAAABBBBBBBBBBBBBBBB
        version = Integer.parseInt(stream.read(3), BINARY);
        typeId = Integer.parseInt(stream.read(3), BINARY);
        lengthTypeId = stream.readBoolean();
        int lengthInBits = lengthTypeId ? 11 : 15;
        subPacketLength = Integer.parseInt(stream.read(lengthInBits), BINARY);

        if (lengthTypeId) {
            subPackets = parseWithPackageCount(stream, subPacketLength);
        } else {
            subPackets = parseWithByteLength(stream, subPacketLength);
        }
    }

    private Packet[] parseWithByteLength(BinaryStream stream, int lengthInBits) {
        int endIndex = stream.getIndex() + lengthInBits;
        List<Packet> packets = new ArrayList<>();

        while (stream.getIndex() < endIndex) {
            packets.add(Packet.parse(stream));
        }
        return packets.toArray(new Packet[0]);
    }

    private Packet[] parseWithPackageCount(BinaryStream stream, int packageCount) {
        List<Packet> packets = new ArrayList<>();
        for (int i = 0; i < packageCount; i++) {
            packets.add(Packet.parse(stream));
        }
        return packets.toArray(new Packet[0]);
    }

    @Override
    public int getVersion() {
        return version;
    }

    public int getTypeId() {
        return typeId;
    }

    public boolean isLengthTypeId() {
        return lengthTypeId;
    }

    public int getSubPacketLength() {
        return subPacketLength;
    }

    public Packet[] getSubPackets() {
        return subPackets;
    }

    @Override
    public int calculateVersion() {
        return version + Arrays.stream(this.subPackets).mapToInt(Packet::calculateVersion).sum();
    }
}