package d16;

import java.math.BigInteger;

public class BinaryStream {

    public static BinaryStream ofHex(String hex) {
        return ofBinary(new BigInteger("1" + hex, 16).toString(2).substring(1));
    }

    public static BinaryStream ofBinary(String binary) {
        return new BinaryStream(new StringBuilder(binary));
    }

    private final StringBuilder stringBuilder;
    private int index;

    public BinaryStream(StringBuilder stringBuilder) {
        this.stringBuilder = stringBuilder;
    }

    public String peak(int bytesCount) {
        return stringBuilder.substring(index, index + bytesCount);
    }

    public boolean readBoolean() {
        return read(1).equals("1");
    }

    public String getString() { return stringBuilder.toString().replaceAll("(.{64})", "$1\n"); }

    public String read(int bytesCount) {
        String result = stringBuilder.substring(index, index + bytesCount);
        index += bytesCount;
        return result;
    }

    public int getIndex() {
        return index;
    }
}
