package d16;

import org.junit.Assert;
import org.junit.Test;

import java.io.IOException;
import java.io.InputStream;
import java.nio.charset.StandardCharsets;
import java.util.Scanner;

public class PacketDecoderTest {

    @Test
    public void testExample1a() {
        PacketDecoder packetDecoder = new PacketDecoder();

        Packet packet = packetDecoder.parseHex("D2FE28");
        Assert.assertTrue("Wrong packet class: " + packet.getClass().getSimpleName(), packet instanceof LiteralValue);

        LiteralValue literalValue = (LiteralValue) packet;
        Assert.assertEquals(6, literalValue.getVersion());
        Assert.assertEquals(4, literalValue.getTypeId());
        Assert.assertEquals(2021, literalValue.getValue());
    }

    @Test
    public void testExample1b() {
        PacketDecoder packetDecoder = new PacketDecoder();

        Packet packet = packetDecoder.parseBinary("110100101111111000101000");
        Assert.assertTrue("Wrong packet class: " + packet.getClass().getSimpleName(), packet instanceof LiteralValue);

        LiteralValue literalValue = (LiteralValue) packet;
        Assert.assertEquals(6, literalValue.getVersion());
        Assert.assertEquals(4, literalValue.getTypeId());
        Assert.assertEquals(2021, literalValue.getValue());
    }

    @Test
    public void testExample1c() {
        PacketDecoder packetDecoder = new PacketDecoder();

        Packet packet = packetDecoder.parseHex("38006F45291200");
        Assert.assertTrue("Wrong packet class: " + packet.getClass().getSimpleName(), packet instanceof Operator);

        Operator operator = (Operator) packet;
        Assert.assertEquals(1, operator.getVersion());
        Assert.assertEquals(6, operator.getTypeId());
        Assert.assertFalse(operator.isLengthTypeId());
        Assert.assertEquals(27, operator.getSubPacketLength());
        Assert.assertEquals(2, operator.getSubPackets().length);

        Packet subPacket = operator.getSubPackets()[0];
        Assert.assertTrue("Wrong packet class: " + subPacket.getClass().getSimpleName(), subPacket instanceof LiteralValue);
        LiteralValue literalValue = (LiteralValue) subPacket;
        Assert.assertEquals(6, literalValue.getVersion());
        Assert.assertEquals(4, literalValue.getTypeId());
        Assert.assertEquals(10, literalValue.getValue());

        subPacket = operator.getSubPackets()[1];
        Assert.assertTrue("Wrong packet class: " + subPacket.getClass().getSimpleName(), subPacket instanceof LiteralValue);
        literalValue = (LiteralValue) subPacket;
        Assert.assertEquals(2, literalValue.getVersion());
        Assert.assertEquals(4, literalValue.getTypeId());
        Assert.assertEquals(20, literalValue.getValue());
    }

    @Test
    public void testExample1d() {
        PacketDecoder packetDecoder = new PacketDecoder();

        Packet packet = packetDecoder.parseBinary("00111000000000000110111101000101001010010001001000000000");
        Assert.assertTrue("Wrong packet class: " + packet.getClass().getSimpleName(), packet instanceof Operator);

        Operator operator = (Operator) packet;
        Assert.assertEquals(1, operator.getVersion());
        Assert.assertEquals(6, operator.getTypeId());
        Assert.assertFalse(operator.isLengthTypeId());
        Assert.assertEquals(27, operator.getSubPacketLength());
        Assert.assertEquals(2, operator.getSubPackets().length);

        Packet subPacket = operator.getSubPackets()[0];
        Assert.assertTrue("Wrong packet class: " + subPacket.getClass().getSimpleName(), subPacket instanceof LiteralValue);
        LiteralValue literalValue = (LiteralValue) subPacket;
        Assert.assertEquals(6, literalValue.getVersion());
        Assert.assertEquals(4, literalValue.getTypeId());
        Assert.assertEquals(10, literalValue.getValue());

        subPacket = operator.getSubPackets()[1];
        Assert.assertTrue("Wrong packet class: " + subPacket.getClass().getSimpleName(), subPacket instanceof LiteralValue);
        literalValue = (LiteralValue) subPacket;
        Assert.assertEquals(2, literalValue.getVersion());
        Assert.assertEquals(4, literalValue.getTypeId());
        Assert.assertEquals(20, literalValue.getValue());
    }

    @Test
    public void testExample1e() {
        PacketDecoder packetDecoder = new PacketDecoder();

        Packet packet = packetDecoder.parseHex("EE00D40C823060");
        Assert.assertTrue("Wrong packet class: " + packet.getClass().getSimpleName(), packet instanceof Operator);

        Operator operator = (Operator) packet;
        Assert.assertEquals(7, operator.getVersion());
        Assert.assertEquals(3, operator.getTypeId());
        Assert.assertTrue(operator.isLengthTypeId());
        Assert.assertEquals(3, operator.getSubPacketLength());
        Assert.assertEquals(3, operator.getSubPackets().length);

        Packet subPacket = operator.getSubPackets()[0];
        Assert.assertTrue("Wrong packet class: " + subPacket.getClass().getSimpleName(), subPacket instanceof LiteralValue);
        LiteralValue literalValue = (LiteralValue) subPacket;
        Assert.assertEquals(2, literalValue.getVersion());
        Assert.assertEquals(4, literalValue.getTypeId());
        Assert.assertEquals(1, literalValue.getValue());

        subPacket = operator.getSubPackets()[1];
        Assert.assertTrue("Wrong packet class: " + subPacket.getClass().getSimpleName(), subPacket instanceof LiteralValue);
        literalValue = (LiteralValue) subPacket;
        Assert.assertEquals(4, literalValue.getVersion());
        Assert.assertEquals(4, literalValue.getTypeId());
        Assert.assertEquals(2, literalValue.getValue());

        subPacket = operator.getSubPackets()[2];
        Assert.assertTrue("Wrong packet class: " + subPacket.getClass().getSimpleName(), subPacket instanceof LiteralValue);
        literalValue = (LiteralValue) subPacket;
        Assert.assertEquals(1, literalValue.getVersion());
        Assert.assertEquals(4, literalValue.getTypeId());
        Assert.assertEquals(3, literalValue.getValue());
    }

    @Test
    public void testExample1f() {
        PacketDecoder packetDecoder = new PacketDecoder();

        Packet packet = packetDecoder.parseBinary("11101110000000001101010000001100100000100011000001100000");
        Assert.assertTrue("Wrong packet class: " + packet.getClass().getSimpleName(), packet instanceof Operator);

        Operator operator = (Operator) packet;
        Assert.assertEquals(7, operator.getVersion());
        Assert.assertEquals(3, operator.getTypeId());
        Assert.assertTrue(operator.isLengthTypeId());
        Assert.assertEquals(3, operator.getSubPacketLength());
        Assert.assertEquals(3, operator.getSubPackets().length);

        Packet subPacket = operator.getSubPackets()[0];
        Assert.assertTrue("Wrong packet class: " + subPacket.getClass().getSimpleName(), subPacket instanceof LiteralValue);
        LiteralValue literalValue = (LiteralValue) subPacket;
        Assert.assertEquals(2, literalValue.getVersion());
        Assert.assertEquals(4, literalValue.getTypeId());
        Assert.assertEquals(1, literalValue.getValue());

        subPacket = operator.getSubPackets()[1];
        Assert.assertTrue("Wrong packet class: " + subPacket.getClass().getSimpleName(), subPacket instanceof LiteralValue);
        literalValue = (LiteralValue) subPacket;
        Assert.assertEquals(4, literalValue.getVersion());
        Assert.assertEquals(4, literalValue.getTypeId());
        Assert.assertEquals(2, literalValue.getValue());

        subPacket = operator.getSubPackets()[2];
        Assert.assertTrue("Wrong packet class: " + subPacket.getClass().getSimpleName(), subPacket instanceof LiteralValue);
        literalValue = (LiteralValue) subPacket;
        Assert.assertEquals(1, literalValue.getVersion());
        Assert.assertEquals(4, literalValue.getTypeId());
        Assert.assertEquals(3, literalValue.getValue());
    }

    @Test
    public void testExample1g() {
        PacketDecoder packetDecoder = new PacketDecoder();

        Packet packet = packetDecoder.parseHex("8A004A801A8002F478");
        Assert.assertTrue("Wrong packet class: " + packet.getClass().getSimpleName(), packet instanceof Operator);

        Operator operator = (Operator) packet;
        Assert.assertEquals(4, operator.getVersion());
        Assert.assertEquals(1, operator.getSubPackets().length);

        Packet subPacket = operator.getSubPackets()[0];
        Assert.assertTrue("Wrong packet class: " + subPacket.getClass().getSimpleName(), subPacket instanceof Operator);
        Operator subOperator = (Operator) subPacket;
        Assert.assertEquals(1, subOperator.getVersion());
        Assert.assertEquals(1, subOperator.getSubPackets().length);

        Packet subSubPacket = subOperator.getSubPackets()[0];
        Assert.assertTrue("Wrong packet class: " + subSubPacket.getClass().getSimpleName(), subSubPacket instanceof Operator);
        Operator subSubOperator = (Operator) subSubPacket;
        Assert.assertEquals(5, subSubOperator.getVersion());
        Assert.assertEquals(1, subSubOperator.getSubPackets().length);

        Packet subSubSubPacket = subSubOperator.getSubPackets()[0];
        Assert.assertTrue("Wrong packet class: " + subSubSubPacket.getClass().getSimpleName(), subSubSubPacket instanceof LiteralValue);
        LiteralValue literalValue = (LiteralValue) subSubSubPacket;
        Assert.assertEquals(6, literalValue.getVersion());

        Assert.assertEquals(16, packet.calculateVersion());
    }

    @Test
    public void testExample1h() {
        PacketDecoder packetDecoder = new PacketDecoder();
        Packet packet = packetDecoder.parseHex("620080001611562C8802118E34");
        Assert.assertEquals(12, packet.calculateVersion());
    }

    @Test
    public void testExample1i() {
        PacketDecoder packetDecoder = new PacketDecoder();
        Packet packet = packetDecoder.parseHex("C0015000016115A2E0802F182340");
        Assert.assertEquals(23, packet.calculateVersion());
    }

    @Test
    public void testExample1j() {
        PacketDecoder packetDecoder = new PacketDecoder();
        Packet packet = packetDecoder.parseHex("A0016C880162017C3686B18A3D4780");
        Assert.assertEquals(31, packet.calculateVersion());
    }

    @Test
    public void testPuzzle1() throws IOException {
        PacketDecoder packetDecoder = new PacketDecoder();
        Packet packet = packetDecoder.parseHex(readInput("input.txt"));
        int result = packet.calculateVersion();
        System.out.println("Puzzle 1: " + result);
        Assert.assertEquals(1007, result);
    }

    private String readInput(String fileName) throws IOException {
        try (InputStream input = getClass().getResourceAsStream(fileName);
             Scanner scanner = new Scanner(input, StandardCharsets.UTF_8)) {
            return scanner.useDelimiter("\\A").next();
        }
    }

    @Test
    public void testExample2a() {
        PacketDecoder packetDecoder = new PacketDecoder();

        Packet packet = packetDecoder.parseHex("D2FE28");
        Assert.assertEquals(2021, packet.calculateValue());
    }

    @Test
    public void testExample2b() {
        PacketDecoder packetDecoder = new PacketDecoder();

        Packet packet = packetDecoder.parseHex("C200B40A82");
        Assert.assertEquals(3, packet.calculateValue());
    }

    @Test
    public void testExample2c() {
        PacketDecoder packetDecoder = new PacketDecoder();

        Packet packet = packetDecoder.parseHex("04005AC33890");
        Assert.assertEquals(54, packet.calculateValue());
    }

    @Test
    public void testExample2d() {
        PacketDecoder packetDecoder = new PacketDecoder();

        Packet packet = packetDecoder.parseHex("880086C3E88112");
        Assert.assertEquals(7, packet.calculateValue());
    }

    @Test
    public void testExample2e() {
        PacketDecoder packetDecoder = new PacketDecoder();

        Packet packet = packetDecoder.parseHex("CE00C43D881120");
        Assert.assertEquals(9, packet.calculateValue());
    }

    @Test
    public void testExample2f() {
        PacketDecoder packetDecoder = new PacketDecoder();

        Packet packet = packetDecoder.parseHex("D8005AC2A8F0");
        Assert.assertEquals(1, packet.calculateValue());
    }

    @Test
    public void testExample2g() {
        PacketDecoder packetDecoder = new PacketDecoder();

        Packet packet = packetDecoder.parseHex("F600BC2D8F");
        Assert.assertEquals(0, packet.calculateValue());
    }

    @Test
    public void testExample2h() {
        PacketDecoder packetDecoder = new PacketDecoder();

        Packet packet = packetDecoder.parseHex("9C005AC2F8F0");
        Assert.assertEquals(0, packet.calculateValue());
    }

    @Test
    public void testExample2i() {
        PacketDecoder packetDecoder = new PacketDecoder();

        Packet packet = packetDecoder.parseHex("9C0141080250320F1802104A08");
        Assert.assertEquals(1, packet.calculateValue());
    }

    @Test
    public void testPuzzle2() throws IOException {
        PacketDecoder packetDecoder = new PacketDecoder();
        Packet packet = packetDecoder.parseHex(readInput("input.txt"));
        long result = packet.calculateValue();
        System.out.println("Puzzle 1: " + result);
        Assert.assertEquals(834151779165L, result);
    }
}
